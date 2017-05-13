using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameplayController.instance.getVelocityY() <= 0)
        {
            //GameplayController.instance.land();
        }
    }
}
