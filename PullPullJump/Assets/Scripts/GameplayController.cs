using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject controller;
    [SerializeField]
    private float forceJump = 10f;
    [SerializeField]
    private float rangePull = 1.5f;
    [SerializeField]
    private LayerMask groundCheckLayer;

    public static GameplayController instance = null;

    private GameObject myController;
    private SpriteRenderer arrowSprite;
    private Vector3 arrowBaseScale;
    private Quaternion arrowBaseRotation;
    private Vector3 vectorForce;
    private float forcePull = 0f;
    private Rigidbody2D rBody;
    private bool grounded = true;
    private bool canJump = false;
    private float previousVelocityY = 0f;
    private TrajectorySimulation laTrajectoire;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        myController = Instantiate(controller, transform.position, Quaternion.identity);
        myController.SetActive(false);

        arrowSprite = arrow.GetComponent<SpriteRenderer>();
        arrowBaseScale = arrow.transform.localScale;
        arrowBaseRotation = arrow.transform.rotation;
        rBody = GetComponent<Rigidbody2D>();
        laTrajectoire = GetComponent<TrajectorySimulation>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if(grounded)
            {
                activateController();
                canJump = true;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            //rajouter canJump
            if (grounded && canJump)
            {
                desactivateController();
                jump();
            }
        }

        Debug.DrawRay(transform.position, new Vector2(0, -0.45f), Color.green);
    }

    private void FixedUpdate()
    {
        if (grounded)
            return;

        if (rBody.velocity.y <= 0 && checkIfGrounded())
        {
            land();
            return;
        }

        previousVelocityY = rBody.velocity.y;
    }

    void activateController()
    {
        vectorForce = Vector3.up;

        //arrowSprite.enabled = true;

        Vector3 mousePos = Input.mousePosition;

        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        objectPos.z = 0f;

        myController.transform.position = objectPos;
        myController.transform.parent = Camera.main.transform;

        myController.SetActive(true);

        AnimationController.instance.startSqueeze(rangePull);
    }

    void desactivateController()
    {
        //arrowSprite.enabled = false;

        myController.SetActive(false);

        arrow.transform.localScale = arrowBaseScale;
        arrow.transform.rotation = arrowBaseRotation;

        AnimationController.instance.resetScale();
    }

    void jump()
    {
        rBody.AddForce(-vectorForce * (1f + ((forceJump - 1f) * (forcePull/rangePull))), ForceMode2D.Impulse);
        AnimationController.instance.jumpSqueeze(forceJump);
        grounded = false;
        canJump = false;
    }

    void land()
    {
        grounded = true;
        AnimationController.instance.landSqueeze(forceJump);
        rBody.velocity = new Vector2(0f, rBody.velocity.y);
        previousVelocityY = 0f;
    }

    bool checkIfGrounded()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, 0.45f, groundCheckLayer);

        if (hit2D)
            return true;
        else
            return false;
    }

    public void moveArrow(float angle, float magnitude)
    {
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.transform.localScale = new Vector3(arrowBaseScale.x + magnitude, arrowBaseScale.y + magnitude, arrowBaseScale.z);
        laTrajectoire.simulatePath(vectorForce, 1f + ((forceJump - 1f) * (forcePull / rangePull)));
    }

    public void setVectorForce(Vector3 vectorDirection)
    {
        vectorForce = vectorDirection.normalized;
        forcePull = vectorDirection.magnitude;
    }

    public float getMaxRangePull()
    {
        return rangePull;
    }

    public float getVelocityY()
    {
        return rBody.velocity.y;
    }

    public float getPreviousVelocityY()
    {
        return previousVelocityY;
    }
}
