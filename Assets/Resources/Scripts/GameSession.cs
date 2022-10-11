using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int score = 0;

    [SerializeField] private TextMeshProUGUI textPlayerLives;
    [SerializeField] private TextMeshProUGUI textPlayerScore;    

    void Awake()
    {
        HideGameOverHUD();
        ShowLevelTitle();
        KeepJustOneGameSession();
    }

    void Start()
    {
        textPlayerLives.text = $"X{playerLives}";
        textPlayerScore.text = $"{score}";
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

    //Shows the level name and after some seconds hides it
    public void ShowLevelTitle()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject levelTitle = canvas.transform.GetChild(0).gameObject;

        //If it is hidden show it
        levelTitle.SetActive(true);

        //Gets the text references
        TextMeshProUGUI textLevelNumber = levelTitle.transform.Find("TextLevelNumber").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textLevelName = levelTitle.transform.Find("TextLevelName").GetComponent<TextMeshProUGUI>();

        //Gets active scene build number
        Scene actualScene = SceneManager.GetActiveScene();
        int levelNumber = actualScene.buildIndex + 1;

        if (levelNumber == 1)
        {
            textLevelNumber.text = "Level 1";
            textLevelName.text = "A fresh journey begins";
        }
        else if (levelNumber == 2)
        {
            textLevelNumber.text = "Level 2";
            textLevelName.text = "Watery waters";
        }
        else if (levelNumber == 3)
        {
            textLevelNumber.text = "Level 3";
            textLevelName.text = "Bastion Nightmare";
        }
    }

    //Hide the current level name after some seconds
    public void HideLevelTitle()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject levelTitle = canvas.transform.GetChild(0).gameObject;
        levelTitle.SetActive(false);
    }

    //Hides the show game over on the screen
    public void HideGameOverHUD()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject gameOverHUD = canvas.transform.GetChild(1).gameObject;

        gameOverHUD.SetActive(false);
    }

    //Shows the game over text on the screen
    public void ShowGameOverHUD()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject gameOverHUD = canvas.transform.GetChild(1).gameObject;

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
            //Makes the scene persist be destroyed for it to hold the info
            //of the new level (enemies and pickups)
            ScenePersist scenePersistHandler = FindObjectOfType<ScenePersist>();
            scenePersistHandler.ResetScenePersist();

            SceneManager.LoadScene(0);
            //Starts the game with a fresh game session
            //(Resets its lifes)
            Destroy(gameObject);
        }
    }

    //Increases the amount of score the player have
    public void AddScore(int pointsScore)
    {
        score += pointsScore;

        //If the player passes the maximum amount of
        //score then it reset back to the maximum
        if (score > 99999)
            score = 99999;

        textPlayerScore.text = $"{score}";
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 0)
        {            
            playerLives -= 1;
        }
        //Shows the game over screen
        ShowGameOverHUD();
        textPlayerLives.text = $"X{playerLives}";
    }
}
