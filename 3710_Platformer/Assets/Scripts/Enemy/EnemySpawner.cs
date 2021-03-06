﻿using UnityEngine;
using System.Collections;
/// <summary>
///	spawn a number (numEnemy) of enemies over certain time span (spawnFrequency)
/// </summary>
public class EnemySpawner : MonoBehaviour
{

    public int numEnemy = 10;  //total number of EnemyType1 that will spawn this stage
    private int enemiesToSpawn;
    public int spawnFrequency = 5;  //how often an enemy spawns (in seconds)
    public GameObject enemyType;    //the type of enemy to be spawned
    public GameObject spawnPoint;
    public int limit = 1;   //Max number of monsters spawned at a time
    private bool complete;
    private int currentlySpawned = 0; //how many monsters are currently spawned
    private float frameRate;
    private int needPortalAnimation = 0;
    // Use this for initialization
    void Start()
    {
        if (numEnemy == 0)
        {
            complete = true;
        }
        enemiesToSpawn = numEnemy;
        spawnPoint.SetActive(false);

        InvokeRepeating("spawnEnemies", spawnFrequency, spawnFrequency);
    }
    // Update is called once per frame
    void Update()
    {
        if (complete)
        {
            complete = false;
            GameObject.Find("LevelManager").GetComponent<LevelManager>().setStageCleared();
        }
    }
    /// <summary>
    /// Spawns one enemy every x seconds until all enemies have spawned
    /// </summary>
    /// <returns></returns>
    private void spawnEnemies()
    {
        if (enemiesToSpawn > 0 && currentlySpawned < limit)
        {
            spawnPoint.SetActive(true);//show portal animation       
            currentlySpawned++;
            enemiesToSpawn--;
            Invoke("instantiateEnemy", 1);

        }
    }
    private void instantiateEnemy()
    {
        GameObject enemy = Instantiate(enemyType, spawnPoint.transform.position, Quaternion.identity, this.transform) as GameObject;
        needPortalAnimation--;
        Invoke("hidePortal", 1);    //hides portal animation after 1 seconds

    }
    /// <summary>
    /// If all enemies have been spawned and there are no enemies currently spawned, complete = true
    /// </summary>
    public void spawnDied()
    {
        currentlySpawned--;
        if (currentlySpawned <= 0 && enemiesToSpawn <= 0)
        {
            complete = true;
        }
    }
    /// <summary>
    /// Displays the portal for 2 seconds
    /// </summary>
    /// <returns></returns>
    private void hidePortal()
    {
        if (needPortalAnimation <= 0)
        {
            spawnPoint.SetActive(false);//hide portal animation 
        }
    }
}