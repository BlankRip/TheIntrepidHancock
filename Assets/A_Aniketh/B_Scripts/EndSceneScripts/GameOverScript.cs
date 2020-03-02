using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;                                     //Setting cursor to be visible when in this scene
        Cursor.lockState = CursorLockMode.None;                   //Making the cursor free to move in the scene
    }

    public void TryAgain()
    {
        SceneShifter.LoadScene("B_Playground");       //Load the game scene
    }

    public void ReturnToMenu()
    {
        SceneShifter.LoadScene("B_Menu");       //Loading the menu scene
    }
}
