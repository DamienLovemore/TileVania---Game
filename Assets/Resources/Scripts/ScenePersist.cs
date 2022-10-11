using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        //Gets how much Scene Persists objects we have
        int numScenePersists = FindObjectsOfType<ScenePersist>().Length;

        //If there is more then one scene persists then, it should destroy this one
        //(Leaving just one instead)
        if (numScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //Makes this object to not be destroied between level loads and restarts
            DontDestroyOnLoad(gameObject);
        }
    }

    //When the player loses all lives, or switch levels it should reset the scene
    //persist object.(It should persist the pickups and enemies of the new level)
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
