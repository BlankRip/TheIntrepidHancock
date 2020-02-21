using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject creditsPannel;
    [SerializeField] GameObject animaticDisplay;
    [SerializeField] float animaticLength;

    public void StartGame()
    {
        animaticDisplay.SetActive(true);
        menuOptions.SetActive(false);
        StartCoroutine(AfterAnimatic());
    }

    public void ShowCredits()
    {
        creditsPannel.SetActive(true);
        menuOptions.SetActive(false);
    }

    public void CloseCredits()
    {
        menuOptions.SetActive(true);
        creditsPannel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator AfterAnimatic()
    {
        yield return new WaitForSecondsRealtime(animaticLength);
        SceneShifter.LoadScene("B_Playground");
    }
}
