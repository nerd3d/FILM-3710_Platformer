using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    private List<GameObject> enemySpawners = new List<GameObject>();
    private int stage;
    private bool currentStageCleared;
    public GameObject text;
    private bool stageStartInProgress;
	// Use this for initialization

    /// <summary>
    /// makes a list of all enemySpawners in scene
    /// </summary>
	void Start ()
    {
        //gets all spawn clusters in current scene.
        List<GameObject> spawnClusters = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnCluster"));

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
        stage = 1;
        StartCoroutine(StartStage());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!stageStartInProgress)
        {
            foreach (GameObject spawner in enemySpawners)
            {
                if (spawner.name.Contains(stage.ToString()) && !spawner.activeSelf)
                {
                    spawner.SetActive(true);
                    spawner.GetComponent<EnemySpawner>().restart();
                }
                else if(!spawner.name.Contains(stage.ToString()) && spawner.activeSelf)
                {
                    spawner.SetActive(false);
                }
            }
        }
    }
    public bool isStageCleared()
    {
        return currentStageCleared;
    }
    IEnumerator StartStage()
    {
        stageStartInProgress = true;
        WaitForSeconds wait = new WaitForSeconds(2);
        text.GetComponent<Text>().text = "Stage " + stage;
        text.SetActive(true);
        yield return wait;//wait 2 seconds
        text.GetComponent<Text>().text = "Start!";
        yield return wait;//wait 2 seconds
        text.SetActive(false);
        stageStartInProgress = false;
    }
}
