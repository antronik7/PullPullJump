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
    private Vector2 bottomPlayer;

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

        bottomPlayer = transform.position;
        bottomPlayer = new Vector2(bottomPlayer.x, (bottomPlayer.y - (GetComponent<Collider2D>().bounds.extents.y)) + 0.01f);
        Debug.DrawRay(bottomPlayer, new Vector2(0, -0.05f), Color.green);
    }

    private void FixedUpdate()
    {
        if (grounded)
            return;

        /*if (rBody.velocity.y <= 0 && checkIfGrounded())
        {
            land();
            return;
        }*/

        previousVelocityY = rBody.velocity.y;
    }

    void activateController()
    {
        vectorForce = Vector3.down;

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
        Debug.Log(vectorForce);
        rBody.AddForce(-vectorForce * (1f + ((forceJump - 1f) * (forcePull/rangePull))), ForceMode2D.Impulse);
        AnimationController.instance.jumpSqueeze(forceJump);
        grounded = false;
        canJump = false;
    }

    public void land()
    {
        grounded = true;
        AnimationController.instance.landSqueeze(forceJump);
        rBody.velocity = new Vector2(0f, rBody.velocity.y);
        previousVelocityY = 0f;
    }

    bool checkIfGrounded()
    {
        bottomPlayer = transform.position;
        bottomPlayer = new Vector2(bottomPlayer.x, (bottomPlayer.y - (GetComponent<Collider2D>().bounds.size.y / 2)) + 0.01f);

        RaycastHit2D hit2D = Physics2D.Raycast(bottomPlayer, Vector2.down, 0.05f, groundCheckLayer);

        if (hit2D && Vector2.Distance(bottomPlayer, hit2D.point) > 0f)
            return true;
        else
            return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(rBody.velocity.y == 0 && collision.gameObject.layer == 8)
        {
            if (!grounded)
                land();
        }
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
