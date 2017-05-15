using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerController : MonoBehaviour {

    [SerializeField]
    private GameObject smallController;
    [SerializeField]
    private float maxAngle;

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

        Vector3 vectorToRotate = new Vector3(0, -allowedPos.magnitude, 0);

        float angleMouse = Vector3.Angle(Vector3.down, allowedPos);
        float angleFoRotation = angleMouse;

        if (allowedPos.x < 0)
            angleFoRotation = angleFoRotation * -1;

        vectorToRotate = Quaternion.AngleAxis(Mathf.Clamp(angleFoRotation, -maxAngle, maxAngle), Vector3.forward) * vectorToRotate;

        Vector3 finalVector = vectorToRotate;

        /*if(angleMouse > maxAngle && allowedPos.magnitude > 0.1f)
        {
            float realY = allowedPos.y;
            float realX = (vectorToRotate.x * realY) / vectorToRotate.y;

            finalVector = new Vector3(realX, realY, 0f);
        }*/

        //smallController.transform.position = transform.position + allowedPos;
        smallController.transform.position = transform.position + finalVector;

        if (smallController.transform.position != previousPosition)
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
