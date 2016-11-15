using UnityEngine;
using System.Collections;

public class Retch : Enemy
{
    //target
    private Transform player;
    public float shootFrequency = 5;
    private float shootCD = 0;//shoot cool-down
    public int range = 6;


    // Use this for initialization
    new void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        shootCD = 0;
    }

    // Update is called once per frame
    new void Update ()
    {
        float thisX = transform.position.x;
        float playerX = player.transform.position.x;
        base.Update();
        if (shootCD <= 0)
        {
            if (_animator.getFacing() == "Left" && thisX > playerX || _animator.getFacing() == "Right" && playerX>thisX)
            {
                shootCD = shootFrequency;
                Debug.Log("Fire!");
            }
        }
        else
        {
            shootCD-=Time.deltaTime;
        }
    }
}
