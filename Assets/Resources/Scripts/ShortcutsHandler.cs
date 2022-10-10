using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShortcutsHandler : MonoBehaviour
{    
    private PlayerMovement playerController;

    void Start()
    {        
        //Handles player info and movement
        playerController = FindObjectOfType<PlayerMovement>();
    }

    void OnShortcutsHandle(InputValue value)
    {
        //If the player hits Esc it should close the game
        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
        }
        //Toggles screen mode
        else if (Keyboard.current.f11Key.isPressed)
        {
            FullScreenMode actualScrenMode = Screen.fullScreenMode;

            //If it is in windowed mode switches to Fullscreen mode
            if ((actualScrenMode == FullScreenMode.Windowed) || (actualScrenMode == FullScreenMode.MaximizedWindow))
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            }
            //If it is on Fullscreen than switches to windowed mode
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }
        //Restart the level on game over
        else if ((Keyboard.current.enterKey.isPressed) && (!playerController.IsPlayerAlive()))
        {
            //Gets the only script of this type in the scene (level)
            GameSession gameSessionHandler = FindObjectOfType<GameSession>();
            gameSessionHandler.RestartLevel();
        }
    }
}
