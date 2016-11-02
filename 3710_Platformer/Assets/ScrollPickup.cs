using UnityEngine;
using System.Collections;

public class ScrollPickup : MonoBehaviour {

    public int bounceHeight = 10;


    private Transform _controller;
    private AnimationController2D _animator;
    private float bounceFrame = 0;
    private bool falling = false;
    private bool isPickup = true;

    // Use this for initialization
    void Start()
    {
        _controller = gameObject.GetComponent<Transform>();
        _animator = gameObject.GetComponent<AnimationController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animatePosition();
        _animator.setAnimation("glow");
    }

    // If player collides and isn't max health, heal'em up
    void OnTriggerEnter2D(Collider2D col)
    {
        
    }

    // animate a little bounce
    void animatePosition()
    {
        Vector3 old = _controller.position;

        if (falling)
        {
            bounceFrame--;
            _controller.position = new Vector3(old.x, old.y - 1 * Time.deltaTime / 4, old.z);
        }
        else
        {
            bounceFrame++;
            _controller.position = new Vector3(old.x, old.y + 1 * Time.deltaTime / 4, old.z);
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
