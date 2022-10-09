using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Rigidbody2D enemyRigidbody;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //Inverts the walking direction when it exits colliding with the
        //plataforms blocks (beggin walking into the wall)
        moveSpeed = -moveSpeed;
        FlipSprite();
    }

    //Makes the enemy faces the direction he is walking into
    //(left or right)
    private void FlipSprite()
    {
        float flipDirection = Mathf.Sign(enemyRigidbody.velocity.x);
        flipDirection = -flipDirection;

        transform.localScale = new Vector2(flipDirection, 1f);
    }
}
