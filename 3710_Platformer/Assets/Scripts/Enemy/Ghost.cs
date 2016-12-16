using UnityEngine;
using System.Collections;

public class Ghost : Pest
{
    private float speed;
    private Vector3 endPoint;
    bool hitLeft = false;
    bool hitRight = false;
    private int effectCounter = 5;
    public override void Start()
    {
        base.Start();
        speed = 8 * Time.fixedDeltaTime;
    }
    public override void Update()
    {
        base.Update();
        if (Time.timeScale != 0)
        {
            if (hitLeft)//headed left
            {
                Vector3 newPosition = transform.position;
                newPosition.x += speed;
                newPosition.y -= speed;
                transform.position = newPosition;
                if (transform.position.x < endPoint.x)
                {
                    transform.position = endPoint;
                    hitLeft = false;
                }
            }
            else if (hitRight)
            {
                Vector3 newPosition = transform.position;
                newPosition.x += speed;
                newPosition.y += speed;
                transform.position = newPosition;
                if (transform.position.x > endPoint.x)
                {
                    transform.position = endPoint;
                    hitRight = false;
                }
            }
        }

    }
    public override void OnTriggerStay2D(Collider2D col)
    {
        if (isEnemy && !hitRight && !hitLeft)//restrict following code to enemy on trigger collission
        {
            if (col.tag == "PlayerClub") // damage effect
            {
                SoundManager.instance.PlaySingle(hit1);
                this.currentHealth -= 1;
                this.checkDeath("Death");
                if (player.position.x < transform.position.x)
                {
                    hitRight = true;
                    endPoint = new Vector3(transform.position.x + 5, transform.position.y + 5);
                    if (speed < 0)
                    {
                        speed *= -1;
                    }
                }
                else
                {
                    hitLeft = true;
                    endPoint = new Vector3(transform.position.x - 5, transform.position.y + 5);
                    if (speed > 0)
                    {
                        speed *= -1;
                    }
                }
            }
        }

    }
}
