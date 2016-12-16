using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour 
{
    static int maxLevels = 19;
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
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(0);
    }
    public void StartLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void SkipIntro()
    {
        //tutorial scene is 15th
        SceneManager.LoadScene(15);
    }
    /// <summary>
    /// Starts the next level. If there is no next level, the player wins (title screen)
    /// </summary>
    public static void StartNextLevel() 
    {
      int nextLevel = SceneManager.GetActiveScene().buildIndex+1;
      
      if(nextLevel<=maxLevels)
        SceneManager.LoadScene(nextLevel);
      else
        SceneManager.LoadScene(0); // Should change to Win Game Screen, when we have one. - Chris
  }
    /// <summary>
    /// Wrapper method for making startnextlevel none-static
    /// This is needed to use the method in unity button ui.
    /// </summary>
    public void StartNextLvlWrapper()
    {
        StartNextLevel();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
  
}
