using UnityEngine;
using System.Collections;

public class ClubAttackBox : MonoBehaviour {

    public GameObject player;
    private AnimationController2D _animator;
    private int swingStart = 44;
    private int swingShow = 38;
    private int swingHide = 30;
    private int swingEnd = 15;
    private int swingTime = 0;
    private bool swingDone = false;

    // Use this for initialization
    void Start () {
        _animator = player.gameObject.GetComponent<AnimationController2D>();
	}
	
	// Update is called once per frame
	void Update () {

        KeepUpWithPlayer();
        CheckAttack();
        
    }

    /// <summary>
    /// Follows players movement and dirrection to keep attack box in front of player.
    /// </summary>
    private void KeepUpWithPlayer()
    {
        transform.position = player.transform.position; // follow the player

        if (player.transform.localScale.x > 0)              // player is facing left
            transform.localScale = new Vector3(-1, 1, 1);
        else if (player.transform.localScale.x < 0)         // player is facing right
            transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Looks at players current animation and activates trigger box while attacking
    /// </summary>
    private void CheckAttack()
    {
        if (!swingDone)
        {
            BoxCollider2D isTrigger = gameObject.GetComponent<BoxCollider2D>();
            SpriteRenderer isRender = gameObject.GetComponent<SpriteRenderer>();

            if (swingTime == 0 && _animator.getAnimation() == "ClubAttack")
                swingTime = swingStart;
            if (swingTime > 0)
            {
                if (swingTime == swingShow)
                {
                    isTrigger.enabled = true;
                    isRender.enabled = true;
                }
                if (swingTime == swingHide)
                {
                    
                    isRender.enabled = false;
                }
                if (swingTime == swingEnd)
                {
                    isTrigger.enabled = false;
                    
                }
                swingTime--;
                if (swingTime == 0)
                    swingDone = true;
            }
        }else
        {
            if (_animator.getAnimation() != "ClubAttack")
                swingDone = false;
        }
    }
}
