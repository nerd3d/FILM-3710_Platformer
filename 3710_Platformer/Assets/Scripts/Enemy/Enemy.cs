﻿using UnityEngine;
using System.Collections;
using Prime31;
using System.Collections.Generic;

/// <summary>
/// This is the base enemy class. This enemy will not chase the player. Movement is simply
/// x-vector.
/// </summary>
public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public CharacterController2D _controller;//unity movement controls
    [HideInInspector]
    public AnimationController2D _animator;//unity animation controls
    [HideInInspector]
    public float currentHealth = 0;//current health of enemy at any given time
    private float cameraWidth;//width of cameraview in units <-super important, see implementation below
    public AudioClip hit1;
    public AudioClip death1;
    public AudioClip death2;
    public AudioClip death3;
    public bool isDead;//determines contactDamage and movement
    [HideInInspector]
    public bool isEnemy;//restrict trigger collission code to enemy

    public int startHP = 5;  //initial hp of this enemy

    //contactDamage may need to be handled by trigger, 
    //may need a level manager for keeping track of enemies for player
    public int contactDamage = 1;//damage player takes when touched by this enemy
    public int stayDamage = 0;//DPS player takes when staying in contact with this enemy
    public int knockback = 5;

    public float moveSpeed = 1;//movement speed of this enemy
    public bool beginFacingRight = true;
    public float gravity = -35;
    [HideInInspector]
    public bool removingEnemy;
    private bool wasPaused = false;

    private Vector3 previousPosition;

    // Use this for initialization
    public void Start ()
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
	virtual public void Update ()
    {
        if(Time.timeScale != 0)
        {
            if (!isDead)
            {
                _controller.move(AiMovement() * Time.deltaTime);
            }
            else
            {
                if (!removingEnemy)
                {
                    removeEnemy();
                }
                removingEnemy = true;
            }
        }
        else
        {
            wasPaused = true;
        }
	}
    /// <summary>
    /// Basic AIMovement is limited to x vector.
    /// </summary>
    /// <returns></returns>
    virtual public Vector3 AiMovement()
    {
        Vector3 velocity = _controller.velocity;
        //if not moving forward (due to obstacle)
        if (_controller.transform.position == previousPosition)
        {
            if (wasPaused)
            {
                wasPaused = false;
                previousPosition.y--;
            }
            else
            {
                if (_animator.getFacing() == "Left")
                {
                    turnRight(velocity);
                }
                else
                {
                    turnLeft(velocity);
                }
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
        previousPosition = _controller.transform.position;
        return velocity;
    }
    private void turnRight(Vector3 velocity)
    {
        velocity.x = moveSpeed;
        _animator.setFacing("Right");
    }
    private void turnLeft(Vector3 velocity)
    {
        velocity.x = -moveSpeed;
        _animator.setFacing("Left");
    }
    /// <summary>
    /// Trigger boxes with an "enter" effect will use this function
    /// </summary>
    /// <param name="col">Collider with effect</param>
    virtual public void OnTriggerStay2D(Collider2D col)
    {
        if(isEnemy)//restrict following code to enemy on trigger collission
        {
            if (col.tag == "PlayerClub") // damage effect
            {

                SoundManager.instance.PlaySingle(hit1);
                this.currentHealth -= 5;
                this.checkDeath("Death"); 
            }
        }
    }
    /// <summary>
    /// Checks current health, setting dead boolean when health hits 0.
    /// Also sets contactDamage to 0, which player should be reading to decide damage
    /// taken upon enemy contact.
    /// </summary>
    public void checkDeath(string deathAnimation)
    {
        if(currentHealth <= 0)
        {
            isDead = true;
            contactDamage = 0;
            _animator.setAnimation(deathAnimation);
            SoundManager.instance.RandomizeSfx(death1, death2, death3);
        }
    }
    /// <summary>
    /// Destroys this gameObject after 1 second.
    /// </summary>
    public void removeEnemy()
    {
        tag = "Untagged";
        transform.parent.gameObject.GetComponentInParent<EnemySpawner>().spawnDied();//interacts with spawner to notify this monster died
        Destroy(this.gameObject,1);
    }
}
