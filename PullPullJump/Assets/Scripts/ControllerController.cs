﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerController : MonoBehaviour {

    [SerializeField]
    private float rangePull = 1.5f;
    [SerializeField]
    private GameObject smallController;

    private Vector3 previousPosition = new Vector3(0f, 0f, 0f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        objectPosition.z = 0f;

        Vector3 allowedPos = objectPosition - transform.position;

        allowedPos = Vector3.ClampMagnitude(allowedPos, rangePull);

        smallController.transform.position = transform.position + allowedPos;

        if(smallController.transform.position != previousPosition)
        {
            Vector3 difference = smallController.transform.position - transform.position;

            if (difference.magnitude > 0.1f)
            {
                float rotZ = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg) - 180;

                GameplayController.instance.moveArrow(rotZ, difference.magnitude);
            }
        }

        previousPosition = smallController.transform.position;
    }
}