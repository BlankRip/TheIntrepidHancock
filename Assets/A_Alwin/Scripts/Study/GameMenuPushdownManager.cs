using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuPushdownManager : MonoBehaviour
{
    public static GameMenuPushdownManager instance;
    Stack<PanelState> pannels = new Stack<PanelState>();
    public PanelState startPanel;

    private void Start()
    {

        instance = this;

        pannels.Push(startPanel);
        pannels.Peek().EnablePannel();
    }

    public void GoToNext(PanelState newState)
    {
        pannels.Peek().DisablePannel();
        pannels.Push(newState);
        pannels.Peek().EnablePannel();
    }

    public void GoToBack()
    {
        pannels.Peek().DisablePannel();
        pannels.Pop();
        pannels.Peek().EnablePannel();
    }

    public void Update()
    {
        if (pannels.Count > 1 && Input.GetKeyDown(KeyCode.P))
        {
            GoToBack();
        }
    }

}
