using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerController : MonoBehaviour {

    [SerializeField]
    private float rangePull = 1.5f;

    [SerializeField]
    private GameObject smallController;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 allowedPos = objectPosition - transform.position;

        allowedPos = Vector3.ClampMagnitude(allowedPos, rangePull);

        smallController.transform.position = transform.position + allowedPos;
    }
}
