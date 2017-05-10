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

    // Number of segments to calculate - more gives a smoother line
    public int segmentCount = 3;
 
	// Length scale for each segment
	public float segmentScale = 1;
 
	// gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
	private Collider _hitObject;
	public Collider hitObject { get { return _hitObject; } }

    private GameObject[] dots;
    private int layerMask = 1 << 9;

    private void Awake()
    {
        dots = new GameObject[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            dots[i] = Instantiate(dot, new Vector2(-10, -10), Quaternion.identity) as GameObject;
        }
    }

    /// <summary>
    /// Simulate the path of a launched ball.
    /// Slight errors are inherent in the numerical method used.
    /// </summary>
    public void simulatePath(Vector3 vectorForce, float forcePull)
	{
        resetTrajectory();

        Vector3 pVelocity = -vectorForce * forcePull;

        float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(pVelocity.y, pVelocity.x));
        float fTime = 0;
        int reflect = 1;

        dots[0].transform.position = transform.position;

        fTime += 0.1f;

        ContactFilter2D contact = new ContactFilter2D();

        contact.SetLayerMask(maskWallBounce);

        for (int i = 1; i < segmentCount; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
            Vector3 pos = new Vector3(transform.position.x + (dx * reflect), transform.position.y + dy, 0);

            dots[i].transform.position = pos;

            Vector2 vectorDirectionRay = dots[i].transform.position - dots[i - 1].transform.position;
            float lenghtRay = vectorDirectionRay.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(dots[i - 1].transform.position, vectorDirectionRay, lenghtRay, maskWallBounce);

            if (hit.collider != null)
            {
                dots[i].transform.position = hit.point;
                Debug.Log(dots[i].transform.position.x);
                break;
            }

            //dots[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude) * fTime, pVelocity.x) * Mathf.Rad2Deg);
            fTime += 0.1f;
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