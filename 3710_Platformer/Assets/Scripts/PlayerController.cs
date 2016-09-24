using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private CharacterController2D _controller;
    private AnimationController2D _animator;
    private float currentHealth = 0;
    private bool PlayerControl = true;

    public GameObject gameOverPanel;
    public GameObject gameCamera;
    public GameObject healthBar;
    public float gravity = -35;
    public float walkSpeed = 3;
    public float jumpHeight = 2;
    public int health = 100;

    // Use this for initialization
    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        _animator = gameObject.GetComponent<AnimationController2D>();
        gameCamera.GetComponent<CameraFollow2D>().startCameraFollow(this.gameObject);

        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerControl)
            _controller.move(PlayerInput() * Time.deltaTime);
    }

    private Vector3 PlayerInput()
    {
        Vector3 velocity = _controller.velocity; // initial velocity

        if (_controller.isGrounded && _controller.ground != null && _controller.ground.tag == "MovingPlatform")
        {
            this.transform.parent = _controller.ground.transform;
        }
        else
        {
            if (this.transform.parent != null)
                transform.parent = null;
        }

        if (Input.GetAxis("Horizontal") < 0)
        { // Move Left
            velocity.x = walkSpeed * -1;
            if (_controller.isGrounded)
                _animator.setAnimation("Run");
            _animator.setFacing("Left");
        }
        else if (Input.GetAxis("Horizontal") > 0)
        { // Move Right
            velocity.x = walkSpeed;
            if (_controller.isGrounded)
                _animator.setAnimation("Run");
            _animator.setFacing("Right");
        }
        else
        {
            if (_controller.isGrounded)
                _animator.setAnimation("Idle");
        }

        if (Input.GetAxis("Jump") > 0 && _controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _animator.setAnimation("Jump");
        }

        velocity.x *= 0.8f; // light friction

        velocity.y += gravity * Time.deltaTime;

        return velocity;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Damaging")
        {
            PlayerDamage(20 * Time.deltaTime);

        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "KillZ")
        {
            PlayerFallDeath();
        }
        else if (col.tag == "Damaging")
        {
            PlayerDamage(20);
        }
    }

    private void PlayerDamage(float damage)
    {
        currentHealth -= damage;
        float normalizedHealth = (float)currentHealth / (float)health;
        GameObject.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth * 256, 32);
        if (currentHealth <= 0)
            PlayerDeath();
    }

    private void PlayerDeath()
    {
        _animator.setAnimation("PlayerDeath");
        PlayerControl = false;
        //gameOverPanel.SetActive(true);
    }

    private void PlayerFallDeath()
    {
        currentHealth = 0;
        GameObject.Find("Health").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 32);
        gameCamera.GetComponent<CameraFollow2D>().stopCameraFollow();
        //gameOverPanel.SetActive(true);
    }
}
