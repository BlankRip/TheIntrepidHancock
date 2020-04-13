using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [Tooltip("The menu panle or all its options")] [SerializeField] GameObject menuOptions;
    [Tooltip("The credits panle")] [SerializeField] GameObject creditsPannel;
    [Tooltip("The animatic display panle")] [SerializeField] GameObject animaticDisplay;
    [Tooltip("The length of the animatic in seconds")] [SerializeField] float animaticLength;

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
        SceneShifter.LoadScene(1);
    }
}
