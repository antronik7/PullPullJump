using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public static AnimationController instance = null;

    private float maxMulSqueeze;
    private float maxMulJump;
    private float baseScaleY;
    private bool doTheJumpSqueeze = false;
    private Animator animator;

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
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		if(doTheJumpSqueeze)
        {
            animator.Play("Jump", 0, Mathf.Clamp(Mathf.Abs(GameplayController.instance.getVelocityY()) / maxMulJump,0f,0.9999f));
        }
	}

    public void squeezeCharacter(float force)
    {
        animator.Play("Squeeze", 0, Mathf.Clamp(force / maxMulSqueeze,0f,0.9999f));
        //anim["Squeeze"].time = force / maxMulSqueeze;
        //transform.localScale = new Vector3(transform.localScale.x, baseScaleY - (maxSqueeze * (force/maxMulSqueeze)), transform.localScale.z);
    }

    public void jumpSqueeze(float maxVelocity)
    {
        maxMulJump = maxVelocity;
        doTheJumpSqueeze = true;
    }

    public void landSqueeze(float maxVelocity)
    {
        Debug.Log(GameplayController.instance.getVelocityY());


        doTheJumpSqueeze = false;
        animator.Play("Land", 0, 0.0833f - (0.0833f * (GameplayController.instance.getVelocityY() / maxVelocity)));
        //animator.SetTrigger("Land");
    }

    public void resetScale()
    {
        //transform.localScale = new Vector3(transform.localScale.x, baseScaleY, transform.localScale.z);
    }

    public void startSqueeze(float maxPull)
    {
        maxMulSqueeze = maxPull;
        //animator.SetTrigger("Squeeze");
    }
}
