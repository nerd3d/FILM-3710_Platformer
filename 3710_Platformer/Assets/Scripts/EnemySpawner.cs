using UnityEngine;
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
    // Use this for initialization
    void Start()
    {
        enemiesToSpawn = numEnemy;
        spawnPoint.SetActive(false);

        //We wait for 2 seconds after spawn animation starts to restart the spawning loop,
        //so we subtract that amount of time from spawnFrequency
        if (spawnFrequency > 2)
        {
            spawnFrequency -= 2;
        }
        StartCoroutine(spawnEnemies());
    }
    public void restart()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// Spawns one enemy every x seconds until all enemies have spawned
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnEnemies()
    {
        while(enemiesToSpawn > 0)
        {
            if (currentlySpawned < limit)
            {          
                Debug.Log("currentlySpawned:" + currentlySpawned);
                yield return new WaitForSeconds(spawnFrequency);
                spawnPoint.SetActive(true);//show portal animation
                yield return new WaitForSeconds(1);
                currentlySpawned++;
                enemiesToSpawn--;
                GameObject enemy = Instantiate(enemyType, spawnPoint.transform.position, Quaternion.identity, this.transform) as GameObject;
                yield return new WaitForSeconds(1);
                spawnPoint.SetActive(false);//hide portal animation 
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
    /// <summary>
    /// If all enemies have been spawned and there are no enemies currently spawned, complete = true
    /// </summary>
    public void spawnDied()
    {
        currentlySpawned--;
        Debug.Log("currentlySpawned:" + currentlySpawned);
        if (currentlySpawned == 0 && numEnemy == 0)
        {
            complete = true;
        }
    }
}