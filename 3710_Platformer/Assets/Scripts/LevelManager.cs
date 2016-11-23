using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    private List<GameObject> enemySpawners = new List<GameObject>();
    private int stage;
    private bool levelCleared;
    public GameObject text;
    private bool stageTransitionInProgress;
    private int stageCleared;
    private int numSpawnClusters;
	// Use this for initialization

    /// <summary>
    /// makes a list of all enemySpawners in scene
    /// </summary>
	void Start ()
    {
        //gets all spawn clusters in current scene.
        List<GameObject> spawnClusters = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnCluster"));
        numSpawnClusters = spawnClusters.Count;
        foreach(GameObject cluster in spawnClusters)
        {
            foreach(Transform child in cluster.transform)
            {
                if(child.gameObject.GetComponent<EnemySpawner>())
                {
                    child.gameObject.SetActive(false);
                    enemySpawners.Add(child.gameObject);
                }
            }
        }
        StartCoroutine(startNextStage());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(stageCleared >= numSpawnClusters)
        {
            stageCleared = 0;
            StartCoroutine(EndStage());
        }
        if(!stageTransitionInProgress)
        {
            foreach (GameObject spawner in enemySpawners)
            {
                if (spawner.name.Contains(stage.ToString()) && !spawner.activeSelf)
                {
                    spawner.SetActive(true);
                }
                else if(!spawner.name.Contains(stage.ToString()) && spawner.activeSelf)
                {
                    spawner.SetActive(false);
                }
            }
        }
    }
    IEnumerator startNextStage()
    {
        stage++;
        stageTransitionInProgress = true;
        WaitForSeconds wait = new WaitForSeconds(2);
        text.GetComponent<Text>().text = "Stage " + stage;
        text.SetActive(true);
        yield return wait;//wait 2 seconds
        text.GetComponent<Text>().text = "Start!";
        yield return wait;//wait 2 seconds
        text.SetActive(false);
        stageTransitionInProgress = false;
    }
    IEnumerator EndStage()
    {
        stageTransitionInProgress = true;
        WaitForSeconds wait = new WaitForSeconds(2);
        text.GetComponent<Text>().text = "Clear!";
        text.SetActive(true);
        yield return wait;//wait 2 seconds
        text.SetActive(false);
        if(stage >= 5)
        {
            levelCleared = true;

            text.GetComponent<Text>().text = "Level Complete!";
            text.SetActive(true);
            yield return wait;//wait 2 seconds
            text.SetActive(false);
            //GameManager g = new GameManager();
            //g.ExitLevel();
            GameManager.StartNextLevel(); // trying out next level - Chris
    }
        else
        {
            StartCoroutine(startNextStage());
        }
    }
    public void setStageCleared()
    {
        stageCleared++;
    }
}
