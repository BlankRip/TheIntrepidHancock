using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [Tooltip("The key that is pressed to peuse and resume the game")] 
    [SerializeField] KeyCode pauseKey;
    [Tooltip("The pause screen gameobject")] 
    [SerializeField] GameObject pauseScreen;
    GameManager gM;                                                       //GameManager Script

    private void Start()
    {
        gM = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            if (!pauseScreen.activeSelf)
            {
                Cursor.visible = true;                                     //Setting cursor to be visible when on pause screen
                Cursor.lockState = CursorLockMode.None;                    //Unloack the cursor to be able to move away from the center of the screen
                Time.timeScale = 0;                                        //Setting game time sacle to 0 so that time stops for the game
                pauseScreen.SetActive(true);                               //Displaying the pause screen
                gM.paused = true;                                             //Set status of game to be paused
            }
            else if (pauseScreen.activeSelf)
                Resume();
        }
    }

    //Function to resume the game when paused
    public void Resume()
    {
        Cursor.visible = false;                                     //Setting cursor to not be visible when playing the game
        Cursor.lockState = CursorLockMode.Locked;                   //Locking the cursor to the center of the screen so that it does not move out of the window
        pauseScreen.SetActive(false);                               //Removing the pause screen from view
        Time.timeScale = 1;                                         //Setting game time scake to 1 so that time moves normally for the game
        gM.paused = false;                                             //Set status of game to be not paused
    }

    //Function that will take the player back to the main menu
    public void BackToMenu()
    {
        Time.timeScale = 1;                                         //Setting game time scake to 1 so that time moves normally for the game
        SceneShifter.LoadScene(0);                                  //Loading the menu scene
    }
}
