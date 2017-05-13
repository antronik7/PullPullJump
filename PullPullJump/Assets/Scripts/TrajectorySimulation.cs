using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// Controls the Laser Sight for the player's aim
/// </summary>
public class TrajectorySimulation : MonoBehaviour
{
    [SerializeField]
    private GameObject dot;
    [SerializeField]
    private LayerMask maskWallBounce;
    [SerializeField]
    private float facteurRebond = 0.5f;
    [SerializeField]
    private int segmentCount = 20;

    private GameObject[] dots;

    private void Awake()
    {
        dots = new GameObject[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            dots[i] = Instantiate(dot, new Vector2(-10, -10), Quaternion.identity) as GameObject;
        }
    }

    public void simulatePath(Vector3 vectorForce, float forcePull)
	{
        Vector3 pVelocity = -vectorForce * forcePull;

        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));

        float fTimeX = 0;
        float fTimeY = 0;
        float positionStartX = transform.position.x;
        float velocitySlower = 1f;
        bool skipOneDot = false;

        dots[0].transform.position = transform.position;

        fTimeX += 0.1f;
        fTimeY += 0.1f;

        //faire que la trajectoire ne depasse pas le sol
        //faire que la trajectoire est limiter a certain angle

        for (int i = 1; i < segmentCount; i++)
        {
            /***Place a dots at the right position***/
            float dx = velocity * fTimeX * Mathf.Cos(angle * Mathf.Deg2Rad);
            dx = dx * velocitySlower;
            float dy = velocity * fTimeY * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTimeY * fTimeY / 2.0f);
            Vector3 pos = new Vector3(positionStartX + dx, transform.position.y + dy, 0);

            dots[i].transform.position = pos;


            /***Check for wall***/
            Vector2 vectorDirectionRay = dots[i].transform.position - dots[i - 1].transform.position;
            float lenghtRay = vectorDirectionRay.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(dots[i - 1].transform.position, vectorDirectionRay, lenghtRay, maskWallBounce);

            if (hit.collider != null && !skipOneDot)
            {
                /***Placing the dot a the point of collision***/
                dx = hit.point.x - positionStartX;
                
                fTimeY = fTimeY - (((Mathf.Abs(pos.x - hit.point.x)) * 0.1f) / lenghtRay);



                dy = velocity * fTimeY * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTimeY * fTimeY / 2.0f);
                pos = new Vector3(positionStartX + dx, transform.position.y + dy, 0);

                dots[i].transform.position = pos;

                //Debug.Log(dots[i].transform.position.y - dots[i - 1].transform.position.y);

                /***Reduce and inverse velocity***/
                velocitySlower = velocitySlower * facteurRebond * -1f;
                positionStartX = hit.point.x;
                fTimeX = 0;
                skipOneDot = true;
            }
            else
            {
                skipOneDot = false;
            }

            //dots[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);

            fTimeX += 0.1f;
            fTimeY += 0.1f;
        }
    }

    public void resetTrajectory()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            dots[i].transform.position = new Vector3(-10, -10, -10);
        }
    }
}