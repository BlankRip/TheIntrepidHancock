using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneShifter.LoadScene(0);       //Loading the menu scene
    }

    public void QuitGame()
    {
        Application.Quit();             //Closes the application
    }
}
