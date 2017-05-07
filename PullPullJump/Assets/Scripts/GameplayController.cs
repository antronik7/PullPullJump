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

    public static GameplayController instance = null;

    private GameObject myController;
    private SpriteRenderer arrowSprite;
    private Vector3 arrowBaseScale;
    private Quaternion arrowBaseRotation;
    private Vector3 vectorForce;
    private float forcePull;
    private Rigidbody2D rBody;
    private bool canJump = false;

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
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if(rBody.velocity.y == 0)
            {
                activateController();
                canJump = true;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            desactivateController();

            if (canJump)
            {
                jump();
                canJump = false;
            }
        }
    }

    void activateController()
    {
        vectorForce = Vector3.up;

        arrowSprite.enabled = true;

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
        arrowSprite.enabled = false;

        myController.SetActive(false);

        arrow.transform.localScale = arrowBaseScale;
        arrow.transform.rotation = arrowBaseRotation;

        AnimationController.instance.resetScale();
    }

    void jump()
    {
        rBody.AddForce(-vectorForce * (1f + ((forceJump - 1f) * (forcePull/rangePull))), ForceMode2D.Impulse);
        Debug.Log(rBody.velocity.magnitude);
        AnimationController.instance.jumpSqueeze(forceJump);
    }

    public void moveArrow(float angle, float magnitude)
    {
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.transform.localScale = new Vector3(arrowBaseScale.x + magnitude, arrowBaseScale.y + magnitude, arrowBaseScale.z);
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
}
