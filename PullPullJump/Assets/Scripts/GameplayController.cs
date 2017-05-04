using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

    [SerializeField]
    private Transform spriteScale;
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject controller;

    public static GameplayController instance = null;

    private GameObject myController;
    private SpriteRenderer arrowSprite;
    private Vector3 arrowBaseScale;

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
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            arrowSprite.enabled = true;
            activateController();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            arrowSprite.enabled = false;
            desactivateController();
        }
    }

    void activateController()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        objectPos.z = 0f;

        myController.transform.position = objectPos;
        myController.transform.parent = Camera.main.transform;

        myController.SetActive(true);
    }

    void desactivateController()
    {
        myController.SetActive(false);
    }

    public void moveArrow(float angle, float magnitude)
    {
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.transform.localScale = new Vector3(arrowBaseScale.x + magnitude, arrowBaseScale.y + magnitude, arrowBaseScale.z);
    }
}
