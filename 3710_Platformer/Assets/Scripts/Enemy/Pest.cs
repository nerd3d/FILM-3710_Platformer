using UnityEngine;
using System.Collections;

public class Pest : Enemy
{
    [HideInInspector]
    public Transform player;
    // Use this for initialization
    public virtual new void Start ()
    {
        player = GameObject.Find("Player").transform;
        base.Start();
    }

    // Update is called once per frame
    public override Vector3 AiMovement()
    {
        Vector3 velocity = _controller.velocity;
        if (_controller.gameObject.transform.position.x < player.transform.position.x)
        {
            velocity.x = moveSpeed;
        }
        else if(_controller.gameObject.transform.position.x > player.transform.position.x)
        {
            velocity.x = -moveSpeed;
        }
        if(_controller.gameObject.transform.position.y < player.transform.position.y)
        {
            velocity.y = moveSpeed;
        }
        else if(_controller.gameObject.transform.position.y > player.transform.position.y)
        {
            velocity.y = -moveSpeed;
        }
        return velocity;
    }
}
