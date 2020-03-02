using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelState : MonoBehaviour
{
    public GameObject panel;
    public PanelState nextPanel;

    public void EnablePannel()
    {
        panel.SetActive(true);
    }

    public void DisablePannel()
    {
        panel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
           if(nextPanel) GameMenuPushdownManager.instance.GoToNext(nextPanel);
        }
    }

}
