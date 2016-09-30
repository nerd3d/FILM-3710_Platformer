using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;

/// <summary>
/// This class is meant to control the player character.
/// It assumes the game is a single player game and may not work
///     with multiple players.
/// </summary>
public class PlayerController : MonoBehaviour
{

    private CharacterController2D _controller;  // Prime31 2D controller
    private AnimationController2D _animator;    // Animation Controller
    private float currentHealth;                // Protected Player Health
    private bool PlayerControl = true;          // Player input toggle

    public GameObject gameOverPanel;
    public GameObject gameCamera;
    public GameObject healthBar;
    public float gravity = -35;                 // gravity for player
    public float walkSpeed = 3;                 // player walk speed
    public float slideFriction = 0.2f;          // sliding friction for player
    public float jumpHeight = 2;                // Jump Height
    public int startingHealth = 100;                    // Player Starting Health

    /// <summary>
    /// Class Start Function. Initializes Controllers and starts the CameraFollow
    /// </summary>
    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        _animator = gameObject.GetComponent<AnimationController2D>();
        gameCamera.GetComponent<CameraFollow2D>().startCameraFollow(this.gameObject);

        currentHealth = startingHealth; // set starting health
    }

    /// <summary>
    /// Frame Update method. Called every frame
    /// </summary>
    void Update()
    {

        if (PlayerControl)
            _controller.move(PlayerInput() * Time.deltaTime); // player movement references PlayerInput
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

        // if jump is pressed & player is grounded, player jumps
        if (Input.GetAxis("Vertical") > 0 && _controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity); // jump height is a scalar manupulation of gravity
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
        if (col.tag == "Damaging") // Damaging effect
        {
            PlayerDamage(20 * Time.deltaTime);

        }

    }

    /// <summary>
    /// Trigger boxes with an "enter" effect will use this function
    /// </summary>
    /// <param name="col">Collider with effect</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("enterTriger");
        if (col.tag == "KillZ") // insta-kill effect
        {
            PlayerFallDeath();
        }
        else if (col.tag == "EnemyType1") // damage effect
        {
            Debug.Log("Damaged!");
            PlayerDamage(startingHealth + 10);
        }
    }

    /// <summary>
    /// Handles Player damage recieved
    /// </summary>
    /// <param name="damage">Ammount of damage recived</param>
    private void PlayerDamage(float damage)
    {
        currentHealth -= damage;
        float normalizedHealth = (float)currentHealth / (float)startingHealth;
        //GameObject.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth * 256, 32);
        if (currentHealth <= 0) // is player dead
            PlayerDeath();
    }

    /// <summary>
    /// Handles Player Death
    /// </summary>
    private void PlayerDeath()
    {
        _animator.setAnimation("PlayerDeath");
        PlayerControl = false;
        //gameOverPanel.SetActive(true); // Disabled until GameOver Panel is implemented
    }

    /// <summary>
    /// Handles Pleayer death due to falling (includes camera Stop Follow)
    /// </summary>
    private void PlayerFallDeath()
    {
        currentHealth = 0;
        GameObject.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 32);
        gameCamera.GetComponent<CameraFollow2D>().stopCameraFollow();
        //gameOverPanel.SetActive(true); // Disabled until GameOver Panel is implemented
    }
}
