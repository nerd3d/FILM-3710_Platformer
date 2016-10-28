using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {

    public int HealAmount = 1;
    public int bounceHeight = 5;


    private Transform _controller;
    private float bounceFrame = 0;
    private bool falling = false;
    private bool isPickup = true;

    // Use this for initialization
    void Start () {
        _controller = gameObject.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        animatePosition();
	}

    // If player collides and isn't max health, heal'em up
    private void OnTriggerEnter(Collider2D col)
    {
        if(isPickup && col.tag == "Player")
        {
            PlayerController thePlayer = col.gameObject.GetComponent<PlayerController>();
            if(thePlayer.getCurrentHealth() < thePlayer.startingHealth)
            {
                thePlayer.addToHealth(HealAmount);
                Destroy(this.gameObject);
            }

        }
    }

    // animate a little bounce
    void animatePosition()
    {
        Vector3 old = _controller.position;
        
        if(falling){
            bounceFrame--;
            _controller.position = new Vector3(old.x, old.y - 1 * Time.deltaTime/2, old.z);
        }
        else{
            bounceFrame++;
            _controller.position = new Vector3(old.x, old.y + 1 * Time.deltaTime/2, old.z);
        }
        if (bounceFrame > bounceHeight)
        {
            falling = true;
            bounceFrame--;
        }
        if (bounceFrame < 0)
        {
            falling = false;
            bounceFrame++;
        }

    }
}
