using UnityEngine;
using System.Collections;

public class ClubAttackBox : MonoBehaviour {

    public GameObject player;
    private AnimationController2D _animator;

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
        if (_animator.getAnimation() == "ClubAttack")
            GetComponent<Collider2D>().enabled = true;
        else
            GetComponent<Collider2D>().enabled = false;
    }
}
