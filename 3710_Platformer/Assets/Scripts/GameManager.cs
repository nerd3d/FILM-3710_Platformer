using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour 
{

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
    public void ExitGame()
    {
        Application.Quit();
    }
}
