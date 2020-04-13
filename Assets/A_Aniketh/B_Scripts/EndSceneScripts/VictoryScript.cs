using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;                                     //Setting cursor to be visible when in this scene
        Cursor.lockState = CursorLockMode.None;                   //Making the cursor free to move in the scene
    }

    public void ReturnToMenu()
    {
        SceneShifter.LoadScene(0);       //Loading the menu scene
    }

    public void QuitGame()
    {
        Application.Quit();             //Closes the application
    }
}
