using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    void ReturnToMenu()
    {
        SceneShifter.LoadScene(0);       //Loading the menu scene
    }
}
