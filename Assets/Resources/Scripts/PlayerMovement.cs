using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathKick = new Vector2(15f, 10f);
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gun;

    private Vector2 moveInput;
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private CapsuleCollider2D playerBodyCollider;
    private BoxCollider2D playerFeetCollider;
    private float gravityStrenght;

    private bool isAlive = true;    

    void Start()
    {
        //Gets the physics of the player, apply the default
        //gravity strenght and stores its value
        playerRigidbody = GetComponent<Rigidbody2D>();
        gravityStrenght = playerRigidbody.gravityScale;
        playerRigidbody.gravityScale = gravityStrenght;

        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();        
    }
        
    void Update()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }        
    }

    void OnFire(InputValue value)
    {
        //If the player is not alive then do nothing
        if (!isAlive)
            return;
        
        //Creates the new bullet
        GameObject newBullet = Instantiate(bulletPrefab, gun.position, transform.rotation);
        //Makes the bullet sprite face the same
        //direction the player is facing
        newBullet.transform.localScale = transform.localScale;
    }

    void OnMove(InputValue value)
    {        
        if (!isAlive)
            return;

        //Gets the direction the player is trying to move into
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
            return;

        //Gets the game ground layer
        int targetLayer = LayerMask.GetMask("Ground");
        //Only jumps when the player is touching the ground
        if(playerFeetCollider.IsTouchingLayers(targetLayer))
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

    //Returns true if the player is climbing a ladder.(Pressing Up or Down Arrows)
    private bool isPlayerClimbing()
    {        
        bool playerIsClimbing;
        playerIsClimbing = Mathf.Abs(moveInput.y) > Mathf.Epsilon;

        return playerIsClimbing;
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

    //Makes the player goes up and down in a ladder.
    //Disables its gravity as well
    private void ClimbLadder()
    {
        //Gets the game climbing layer
        int targetLayer = LayerMask.GetMask("Climbing");
        //Only jumps when the player is climbing a ladder
        if (playerFeetCollider.IsTouchingLayers(targetLayer))
        {
            //Prevents player from slowly falling when climbing a ladder
            playerRigidbody.gravityScale = 0;
            // playerRigidbody.velocity.x does not change horizontally.
            //Listen movement for up and down
            Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);
            playerRigidbody.velocity = climbVelocity;
            playerAnimator.SetBool("isClimbing", isPlayerClimbing());
        }
        //If it is not climbing anymore sets the gravity again, and changes
        //its animations back to idle
        else
        {
            playerRigidbody.gravityScale = gravityStrenght;
            playerAnimator.SetBool("isClimbing", false);
        }        
    }

    //On player contact with an enemy it "dies"
    //(Disable its controls, and play hit animation)
    private void Die()
    {
        //Used to get all the layers that you want to detect with a rigidbody is
        //colliding with
        int targetLayer = LayerMask.GetMask("Enemies", "Hazards");

        if (playerBodyCollider.IsTouchingLayers(targetLayer))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");

            //Flings the player in the air, after bumping into a enemy
            //To make it more dramatic
            playerRigidbody.velocity = deathKick;

            //Gets the only script of this type in the scene (level)
            GameSession gameSessionHandler = FindObjectOfType<GameSession>();
            gameSessionHandler.Invoke("ProcessPlayerDeath", 1.3f);
        }
    }

    //Returns if the player is alive or not
    public bool IsPlayerAlive()
    {
        bool returnValue;

        returnValue = isAlive;

        return returnValue;
    }
}
