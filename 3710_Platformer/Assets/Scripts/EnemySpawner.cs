using UnityEngine;
using System.Collections;
/// <summary>
///	spawn a number (numEnemy) of enemies over certain time span (spawnFrequency)
/// </summary>
public class EnemySpawner : MonoBehaviour
{

    public int numEnemy = 10;  //total number of EnemyType1 that will spawn this stage
    public int spawnFrequency = 2;  //how often an enemy spawns (in seconds)
    public GameObject enemyType;    //the type of enemy to be spawned
    public GameObject spawnPoint;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(spawnEnemies());
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
        for (int i = 0; i < numEnemy; i++)
        {

            Vector3 position = new Vector3(i * 2.0f, 0.55f, 2.0f);
            yield return new WaitForSeconds(spawnFrequency);
            GameObject enemy = Instantiate(enemyType,spawnPoint.transform.position,Quaternion.identity) as GameObject;
        }
    }
}