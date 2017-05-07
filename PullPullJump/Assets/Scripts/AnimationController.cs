using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    [SerializeField]
    private float maxSqueeze = 0.5f;

    public static AnimationController instance = null;

    private float maxMulSqueeze;
    private float baseScaleY;
    private bool doTheJumpSqueeze = false;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        baseScaleY = transform.localScale.y;
    }

    // Use this for initialization
    void Start () {
        maxMulSqueeze = GameplayController.instance.getMaxRangePull();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void squeezeCharacter(float force)
    {
        transform.localScale = new Vector3(transform.localScale.x, baseScaleY - (maxSqueeze * (force/maxMulSqueeze)), transform.localScale.z);
    }

    public void jumpSqueeze()
    {

    }

    public void resetScale()
    {
        transform.localScale = new Vector3(transform.localScale.x, baseScaleY, transform.localScale.z);
    }
}
