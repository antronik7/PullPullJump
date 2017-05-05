using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    private Rigidbody2D playerRbody;

    // Use this for initialization
    void Awake()
    {
        playerRbody = player.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(player.transform.position.y > transform.position.y && playerRbody.velocity.y > 0)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
	}
}
