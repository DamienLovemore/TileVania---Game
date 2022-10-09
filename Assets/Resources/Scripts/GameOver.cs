using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameoverHUD;
    
    //Shows the game over text on the screen
    public void ShowGameOverHUD()
    {        
        gameoverHUD.SetActive(true);
    }

    //Reload the level for playing again
    public void RestartLevel()
    {
        //Gets a reference to the current scene (level)
        Scene actualScene = SceneManager.GetActiveScene();

        //Loads the target scene
        SceneManager.LoadScene(actualScene.buildIndex);
    }
}
