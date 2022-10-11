using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float waitTime = 1f;

    //If something with a collider entered it
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine("LevelComplete");
        }
    }

    //Load the next level of the game
    IEnumerator LevelComplete()
    {
        //Waits for the specified amount of seconds
        yield return new WaitForSecondsRealtime(waitTime);
        //Load the next level
        Scene actualScene = SceneManager.GetActiveScene();
        int levelNumber = actualScene.buildIndex;

        //If it is not on the final level load the next level
        if (levelNumber != SceneManager.sceneCountInBuildSettings-1)
        {
            //Makes the scene persist be destroyed for it to hold the info
            //of the new level (enemies and pickups)
            ScenePersist scenePersistHandler = FindObjectOfType<ScenePersist>();
            scenePersistHandler.ResetScenePersist();

            SceneManager.LoadScene(levelNumber + 1);
        }
        //If it is just close the game
        else
        {
            Application.Quit();
        }       
    }
}
