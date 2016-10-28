using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;
using System;

/// <summary>
/// This class is meant to control the player character.
/// It assumes the game is a single player game and may not work
///     with multiple players.
/// </summary>
public class PlayerController : MonoBehaviour
{

    private CharacterController2D _controller;  // Prime31 2D controller
    private AnimationController2D _animator;    // Animation Controller
    private int currentHealth;                // Protected Player Health
    private bool playerAlive = true;          // Player input toggle
    private bool isPlayer = true;               // restricts trigger collission code to player
    private int knockback = 0;                  // knockback Force
    private bool knockbackLeft;                 // Direction of knockback
    private float knockbackTime = 0.3f;         // Durration of knockback force
    private float knockbackCount = 0;           // knockback Counter
    private float invulnerable = 0;
    private int flickerFrames = 0;
    private int flickerTotal = 3;
    private SpriteRenderer _sprite;


    public GameObject gameOverPanel;
    public GameObject gameCamera;
    public float gravity = -35;                 // gravity for player
    public float walkSpeed = 3;                 // player walk speed
    public float slideFriction = 0.2f;          // sliding friction for player
    public float jumpHeight = 2;                // Jump Height
    public int startingHealth = 100;            // Player Starting Health
    public int damageDownTime = 1;                // Invulnerablility time after taking damage

    /// <summary>
    /// Class Start Function. Initializes Controllers and starts the CameraFollow
    /// </summary>
    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        _animator = gameObject.GetComponent<AnimationController2D>();
        gameCamera.GetComponent<CameraFollow2D>().startCameraFollow(this.gameObject);
        currentHealth = startingHealth; // set starting health
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Frame Update method. Called every frame
    /// </summary>
    void Update()
    {
        if (knockbackCount > 0)
        {
            if (knockbackLeft)
                _controller.move(new Vector3(-knockback, knockback, 0) * Time.deltaTime);
            else
                _controller.move(new Vector3(knockback, knockback, 0) * Time.deltaTime);
            knockbackCount -= Time.deltaTime;
        }else if (playerAlive && _animator.getAnimation()!="ClubAttack")
        {
            _controller.move(PlayerInput() * Time.deltaTime); // player movement references PlayerInput
        }else if(!playerAlive)
        {
            Vector3 velocity = _controller.velocity;
            velocity.x *= (1 - slideFriction);
            velocity.y += gravity * Time.deltaTime;
            _controller.move(velocity * Time.deltaTime);
            PlayerDeath();
        }
        if (invulnerable > 0)
        {
            tickInvulnerability();
        }
        else if (!_sprite.enabled)
        {
            _sprite.enabled = true;
        }
    }

    private void tickInvulnerability()
    {
        if (flickerFrames <= 0)
        {
            _sprite.enabled = !_sprite.enabled;
            flickerFrames = flickerTotal;
        }else
        {
            flickerFrames--;
        }

        invulnerable -= Time.deltaTime;
    }

    /// <summary>
    /// Looks at player input and determines a movement vector
    /// </summary>
    /// <returns>Movement Vector (velocity)</returns>
    private Vector3 PlayerInput()
    {
        Vector3 velocity = _controller.velocity; // initial velocity

        // If player lands on a moving platform, child the player to that platform, so he'll move with it
        if (_controller.isGrounded && _controller.ground != null && _controller.ground.tag == "MovingPlatform")
        {
            this.transform.parent = _controller.ground.transform;
        }
        else
        {
            if (this.transform.parent != null)
                transform.parent = null;
        }

        // Use Spacebar as attack
        if (Input.GetAxis("Fire1") > 0 && _controller.isGrounded)
        {
            if (_animator.getAnimation() != "ClubAttack")
            {
                _animator.setAnimation("ClubAttack");
            }
            if (Input.GetAxis("Horizontal") < 0)
            { // Face Left
                _animator.setFacing("Left");
            }
            else if (Input.GetAxis("Horizontal") > 0)
            { // Face Right
                _animator.setFacing("Right");
            }
        }
        else
        // if jump is pressed & player is grounded, player jumps
        if (Input.GetAxis("Jump") > 0 && _controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity); // jump height is a scalar manupulation of gravity
        } else 
        // if horizontal input is negative, move left
        if (Input.GetAxis("Horizontal") < 0)
        { // Move Left
            velocity.x = walkSpeed * -1;
            if (_controller.isGrounded)
                _animator.setAnimation("Walk");
            _animator.setFacing("Left");
        }
        // if horizontal input is positive, move right
        else if (Input.GetAxis("Horizontal") > 0)
        { // Move Right
            velocity.x = walkSpeed;
            if (_controller.isGrounded)
                _animator.setAnimation("Walk");
            _animator.setFacing("Right");
        }
        // else, player is idle
        else
        {
            if (_controller.isGrounded)
                _animator.setAnimation("Idle");
        }

        // if in the air, set fump/fall animation
        if (!_controller.isGrounded)
        {
            _animator.setAnimation("Jump");
        }
        
        velocity.x *= (1 - slideFriction); // apply friction

        velocity.y += gravity * Time.deltaTime; // apply gravity

        return velocity; // return the new velicity vector
    }

    /// <summary>
    /// Trigger boxes with a "sustained" effect will use this function
    /// </summary>
    /// <param name="col">Collider with effect</param>
    void OnTriggerStay2D(Collider2D col)
    {
        // in case player becomes vulnerable while touching enemy
        if (isPlayer && invulnerable <= 0)
        {
            if (col.tag == "EnemyType1") // damage effect
            {
                OnTriggerEnter2D(col);
            }
        }

    }

    /// <summary>
    /// Trigger boxes with an "enter" effect will use this function
    /// </summary>
    /// <param name="col">Collider with effect</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if(isPlayer && invulnerable<=0)//(added by adam) restricts following trigger collission code to the player.
        {
            if (col.tag == "KillZ") // insta-kill effect
            {
                PlayerFallDeath();
            }
            else if (col.tag == "EnemyType1") // damage effect
            {
                if (playerAlive)
                {
                    Enemy enemy = col.gameObject.GetComponent<Enemy>();
                    knockback = enemy.knockback;
                    knockbackLeft = (col.transform.position.x - transform.position.x) > 0;
                    knockbackCount = knockbackTime;
                    invulnerable = damageDownTime;

                    _animator.setAnimation("Damaged");
                    int dmg = enemy.contactDamage;
                    PlayerDamage(dmg);
                }
            }
        }
    }

    /// <summary>
    /// Handles Player damage recieved
    /// </summary>
    /// <param name="damage">Ammount of damage recived</param>
    private void PlayerDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;
        if (currentHealth <= 0) // is player dead
            PlayerDeath();
    }

    /// <summary>
    /// Handles Player Death
    /// </summary>
    private void PlayerDeath()
    {
        if(_controller.isGrounded)
            _animator.setAnimation("Death");
        playerAlive = false;
        gameOverPanel.SetActive(true); 
    }

    /// <summary>
    /// Handles Pleayer death due to falling (includes camera Stop Follow)
    /// </summary>
    private void PlayerFallDeath()
    {
        currentHealth = 0;
        gameCamera.GetComponent<CameraFollow2D>().stopCameraFollow();
        gameOverPanel.SetActive(true); 
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public void addToHealth(int amount)
    {
        currentHealth += amount;
        if(currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
    }
}
