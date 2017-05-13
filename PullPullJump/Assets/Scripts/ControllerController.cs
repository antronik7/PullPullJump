using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerController : MonoBehaviour {

    [SerializeField]
    private GameObject smallController;

    private float rangePull;
    private Vector3 previousPosition = new Vector3(0f, 0f, 0f);
    private Vector3 previousMosPos;

    private void Awake()
    {
        rangePull = GameplayController.instance.getMaxRangePull();    
    }

    // Use this for initialization
    void Start () {
		
	}

    private void OnEnable()
    {
        previousMosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        smallController.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update () {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 objectPosition = smallController.transform.position + (mousePos - previousMosPos);
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

                GameplayController.instance.setVectorForce(difference);
                GameplayController.instance.moveArrow(rotZ, difference.magnitude);
                AnimationController.instance.squeezeCharacter(difference.magnitude);
            }
        }

        previousPosition = smallController.transform.position;
        previousMosPos = mousePos;
    }
}
