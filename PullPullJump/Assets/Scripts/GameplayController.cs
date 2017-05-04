using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

    [SerializeField]
    private Transform spriteScale;
    [SerializeField]
    private GameObject controller;

    private GameObject myController;

    void Awake()
    {
        myController = Instantiate(controller, transform.position, Quaternion.identity);
        myController.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            activateController();
        }
        else if(Input.GetMouseButtonUp(0))
        {
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
}
