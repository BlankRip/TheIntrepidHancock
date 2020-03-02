using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneShifter
{
    public static void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
