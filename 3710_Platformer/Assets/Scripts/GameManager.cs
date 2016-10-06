﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RestartLevel()
    {
        string level = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(level);
    }

    public void ExitLevel()
    {
        Application.Quit();
    }
}
