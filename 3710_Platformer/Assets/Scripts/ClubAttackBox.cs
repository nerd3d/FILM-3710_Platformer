using UnityEngine;
using System.Collections;

public class ClubAttackBox : MonoBehaviour {

    public GameObject player;
    private CharacterController pControl;

	// Use this for initialization
	void Start () {
        pControl = player.gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.position = player.transform.position; // follow the player

        if (player.transform.localScale.x > 0)              // player is facing left
            transform.localScale = new Vector3(-1, 1, 1); 
        else if (player.transform.localScale.x < 0)         // player is facing right
            transform.localScale = new Vector3(1, 1, 1);

        if (Input.GetAxis("Jump")>0)
            tag = "PlayerClub";
        else
            tag = "Untagged";
	}
}
