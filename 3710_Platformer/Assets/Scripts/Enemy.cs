﻿using UnityEngine;
using System.Collections;
using Prime31;
using System.Collections.Generic;

/// <summary>
/// This is the base enemy class. This enemy will not chase the player. Movement is simply
/// x-vector and assumes there are no obstacles on the ground floor. 
/// Base enemy will walk across floor until reaching a side of the screen, then turn around.
/// If we wish to add walls they will
/// need tags so the _controller can check for grounded with walls before changing direction (i think).
/// </summary>
public class Enemy : MonoBehaviour
{
    private CharacterController2D _controller;//unity movement controls
    private AnimationController2D _animator;//unity animation controls
    private float currentHealth = 0;//current health of enemy at any given time
    private float cameraWidth;//width of cameraview in units <-super important, see implementation below
    public bool isDead;//determines contactDamage and movement
    private bool isEnemy;//restrict trigger collission code to enemy

    //public GameObject player;//target this enemy will chase/attack
    public int startHP = 50;  //initial hp of this enemy

    //contactDamage may need to be handled by trigger, 
    //may need a level manager for keeping track of enemies for player
    public int contactDamage = 50;//damage player takes when touched by this enemy
    public int stayDamage = 20;//DPS player takes when staying in contact with this enemy

    public float moveSpeed = 1;//movement speed of this enemy
    public bool beginFacingRight = true;
    public float gravity = -35;

    private Vector3 previousPosition;

    // Use this for initialization
    void Start ()
    {

        _controller = gameObject.GetComponent<CharacterController2D>();
        _animator = gameObject.GetComponent<AnimationController2D>();
        currentHealth = startHP;
        if(beginFacingRight)
            _animator.setFacing("Right");
        else
            _animator.setFacing("Left");
        cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        previousPosition = _controller.transform.position;
        previousPosition.y = -1000;//offset previous position for first Update().
        isDead = false;
        isEnemy = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!isDead)
        {
            _controller.move(AiMovement() * Time.deltaTime);
        }
        else
        {
            removeEnemy();
        }
	}
    /// <summary>
    /// Basic AIMovement is limited to x vector.
    /// </summary>
    /// <returns></returns>
    private Vector3 AiMovement()
    {
        Vector3 velocity = _controller.velocity;
        //if offscreen on right side
        if (_controller.transform.position.x > cameraWidth)
        {
            turnLeft(velocity);
        }
        //if offscreen on left side
        else if(_controller.transform.position.x < -cameraWidth)
        {
            turnRight(velocity);
        }
        //if not moving forward (due to obstacle)
        else if(_controller.transform.position == previousPosition)
        {
            if(_animator.getFacing() == "Left")
            {
                turnRight(velocity);
            }
            else
            {
                turnLeft(velocity);
            }
        }
        //move left if facing left, right otherwise.
        if(_animator.getFacing() == "Left")
        {
            velocity.x = -moveSpeed;//move left
        }
        else
        {
            velocity.x = moveSpeed;//move right
        }
        velocity.y += gravity * Time.deltaTime;

        //the following is for a chaser enemy if implemented in future, in progress..
        //   if (_controller.gameObject.transform.position.x < player.transform.position.x)
        //{
        //velocity.x = moveSpeed;
        //_animator.setAnimation("Walk"); //not in whitebox version
        //_animator.setFacing("Right");
        //}
        //else
        //{
        //    velocity.x = -moveSpeed;
        //_animator.setAnimation("Walk"); //not in whitebox version
        //    _animator.setFacing("Left");
        //}
        previousPosition = _controller.transform.position;
        return velocity;
    }
    private void turnRight(Vector3 velocity)
    {
        velocity.x = moveSpeed;
        //_animator.setAnimation("Walk"); //not in whitebox version
        _animator.setFacing("Right");
    }
    private void turnLeft(Vector3 velocity)
    {
        velocity.x = -moveSpeed;
        //_animator.setAnimation("Walk"); //not in whitebox version
        _animator.setFacing("Left");
    }
    /// <summary>
    /// Trigger boxes with an "enter" effect will use this function
    /// </summary>
    /// <param name="col">Collider with effect</param>
    void OnTriggerStay2D(Collider2D col)
    {
        if(isEnemy)//restrict following code to enemy on trigger collission
        {
            if (col.tag == "PlayerClub") // damage effect
            {
                this.currentHealth-= 5*Time.deltaTime; //takes 5 DpS
                this.checkDeath(); 
            }
        }
    }
    /// <summary>
    /// Checks current health, setting dead boolean when health hits 0.
    /// Also sets contactDamage to 0, which player should be reading to decide damage
    /// taken upon enemy contact.
    /// </summary>
    private void checkDeath()
    {
        if(currentHealth <= 0)
        {
            isDead = true;
            contactDamage = 0;
            _animator.setAnimation("SlinkDeath");
        }
    }
    /// <summary>
    /// Destroys this gameObject after 1 second.
    /// </summary>
    void removeEnemy()
    {
        tag = "Untagged";
        Destroy(this.gameObject,1);
    }
}
