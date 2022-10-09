using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;

    private Rigidbody2D bulletRigidbody;
    private GameObject player;
    private float xSpeed;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        //Makes the bullet goes into the direction the player
        //is facing with the determined speed
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        bulletRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    //Handles "collision" with hazards (things that kill the player)
    void OnTriggerEnter2D(Collider2D collision)
    {
        //If it hits an enemy it destroy the enemy
        if (collision.tag == "Enemies")
        {
            Destroy(collision.gameObject);
        }
        //If it hits anything it destroys itself
        Destroy(gameObject);
    }

    //Handles collision with anything that is not a hazard.
    //(Walls, plataforms, ground and stuff like that)
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
