using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] KeyCode pauseKey;                                    //The key that is pressed to peuse and resume the game
    [SerializeField] GameObject pauseScreen;                              //The pause screen gameobject

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
    }

    //Function that will take the player back to the main menu
    public void BackToMenu()
    {
        Time.timeScale = 1;                                         //Setting game time scake to 1 so that time moves normally for the game
        SceneManager.LoadScene(0);                                  //Loading the menu scene
    }
}
