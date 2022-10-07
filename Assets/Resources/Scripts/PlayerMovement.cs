using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;

    private Vector2 moveInput;
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private CapsuleCollider2D playerCapsuleCollider;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }
        
    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        //Gets the direction the player is trying to move into
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        //Gets the game ground layer
        int targetLayer = LayerMask.GetMask("Ground");
        //Only jumps when the player is touching the ground
        if(playerCapsuleCollider.IsTouchingLayers(targetLayer))
        {
            if (value.isPressed)
            {
                playerRigidbody.velocity += new Vector2(0f, jumpForce);
            }
        }        
    }

    //Returns true if the player is moving.(Pressing Left or Right Arrows)
    private bool isPlayerMoving()
    {
        //Checks if the player is moving (if it is a value greater than zero,
        //not taking into account the sign of it)
        //Uses epsilon cause unity generated float values will never be
        //exactly zero, even if they are really low.(Epsilon is smallest
        //possible value for zero)
        bool playerHasMovement;
        playerHasMovement = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

        return playerHasMovement;
    }

    //Makes the player moves horizontally
    private void Run()
    {
        //Move just on the horizontal axis(x), and ignore the y value.
        //Acelerates the speed by the runSpeed factor. (Otherwise it would be too slow)
        //Makes gravity behaves normally on the y, instead of floating in the air
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;

        //Sets the animation trigger, to be played when moving and stop
        //when its not supposed to be moving
        playerAnimator.SetBool("isRunning", isPlayerMoving());
    }

    //Makes the player faces the direction he is walking into
    //(left or right)
    private void FlipSprite()
    {
        if (isPlayerMoving())
        {
            //Math.Sign returns 1 if it is zero or positive, -1 otherwise
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }        
    }
}
