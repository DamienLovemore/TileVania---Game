using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForPickup = 100;

    //Prevents from collecting coin twice
    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player "collect" the coin
        if ((collision.tag == "Player") && (!wasCollected))
        {
            wasCollected = true;
            //Adds the amount of score for this coin
            GameSession gameSessionHandler = FindObjectOfType<GameSession>();
            gameSessionHandler.AddScore(pointsForPickup);

            //Uses PlayClipAtPoint to make the sound still be played after the game object is destroyed
            //The main camera is what listen to audios so different positions, will make the sound be
            //weaker or louder. To be always the same use tha main camera position
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
