using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    void ReturnToMenu()
    {
        SceneManager.LoadScene(0);       //Loading the menu scene
    }
}
