﻿using UnityEngine;
using System.Collections;

public class Retch : Enemy
{
    //target
    private Transform player;
    public float shootFrequency = 5;
    private float shootCD = 0;//shoot cool-down
    public int range = 6;
    public GameObject ammo;

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
                Vector3 modifiedPosition = transform.position;
                modifiedPosition.y += .3f;
                if (_animator.getFacing() == "Right")
                {
                    modifiedPosition.x += .9f;
                }
                else
                {
                    modifiedPosition.x -= .9f;
                }
                GameObject projectile = Instantiate(ammo, modifiedPosition, Quaternion.identity, this.transform) as GameObject;
            }
        }
        else
        {
            shootCD-=Time.deltaTime;
        }
    }
}
