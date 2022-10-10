using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player "collect" the coin
        if (collision.tag == "Player")
        {
            //Uses PlayClipAtPoint to make the sound still be played after the game object is destroyed
            //The main camera is what listen to audios so different positions, will make the sound be
            //weaker or louder. To be always the same use tha main camera position
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
