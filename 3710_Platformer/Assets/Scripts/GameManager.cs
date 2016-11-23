using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour 
{
    static int maxLevels = 2;
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
        SceneManager.LoadScene(0);
    }
    public void StartLevel1()
    {
        //SceneManager.GetActiveScene().buildIndex //can be used to get currentscene index and
        //therefore we can make a startNextLevel() method.
        //-Adam

        SceneManager.LoadScene(1);
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
    public void ExitGame()
    {
        Application.Quit();
    }
}
