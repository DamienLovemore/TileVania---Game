using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;

    void Awake()
    {
        KeepJustOneGameSession();
    }

    //Controls the amount of Game Session objects that only one per level
    // is available. And that it is not erased between level loads
    private void KeepJustOneGameSession()
    {
        //Gets how much Game Session objects we have
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        //If there is more then one game session then, it should destroy this one
        //(Leaving just one instead)
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //Makes this object to not be destroied between level loads and restarts
            DontDestroyOnLoad(gameObject);
        }
    }

    //Shows the game over text on the screen
    public void ShowGameOverHUD()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject gameOverHUD = canvas.transform.GetChild(0).gameObject;

        gameOverHUD.SetActive(true);
       
        //If the player has no more retry chances than it should change the text
        //saying it will have to retry the level again
        if (playerLives == 0)
        {
            //Gets the child of the game over HUD that is responsible for showing
            //game over description
            Transform gameOverDescription = gameOverHUD.transform.Find("TextGameOverDescription");
            //Gets the component responsible for showing text
            TextMeshProUGUI textGameOverDescription = gameOverDescription.GetComponent<TextMeshProUGUI>();
            textGameOverDescription.text = "Press ENTER to begin from level 1";
        }
    }

    //Reload the level for playing again
    public void RestartLevel()
    {
        //If the player still have some of the lives remaining then it
        //restart the level
        if (playerLives > 0)
        {
            //Gets a reference to the current scene (level)
            Scene actualScene = SceneManager.GetActiveScene();

            //Loads the target scene
            SceneManager.LoadScene(actualScene.buildIndex);
        }
        //Otherwise begins the game all again
        else
        {
            Debug.Log("Aqui");
            SceneManager.LoadScene(0);
            //Starts the game with a fresh game session
            //(Resets its lifes)
            Destroy(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 0)
        {            
            playerLives -= 1;
        }
        //Shows the game over screen
        ShowGameOverHUD();
    }
}
