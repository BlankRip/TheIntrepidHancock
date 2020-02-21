using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public void TryAgain()
    {
        SceneShifter.LoadScene("B_Playground");       //Load the game scene
    }

    public void ReturnToMenu()
    {
        SceneShifter.LoadScene(0);       //Loading the menu scene
    }
}
