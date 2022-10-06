using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
        
    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    //Makes the player moves horizontally
    private void Run()
    {
        //Move just on the horizontal axis(x), and ignore the y value.
        //Acelerates the speed by the runSpeed factor. (Otherwise it would be too slow)
        //Makes gravity behaves normally on the y, instead of floating in the air
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;
    }

    //Makes the player faces the direction he is walking into
    //(left or right)
    private void FlipSprite()
    {
        //Checks if the player is moving (if it is a value greater than zero,
        //not taking into account the sign of it)
        //Uses epsilon cause unity generated float values will never be
        //exactly zero, even if they are really low.(Epsilon is smallest
        //possible value for zero)
        bool playerHasMovement = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

        if (playerHasMovement)
        {
            //Math.Sign returns 1 if it is zero or positive, -1 otherwise
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }        
    }
}
