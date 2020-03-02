using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTracker : MonoBehaviour
{
    [HideInInspector] public int numberSwitched;
    [SerializeField] GameObject showPannel;
    bool displayedVictory = false;

    void Update()
    {
        if (numberSwitched >= 33 && !displayedVictory)
        {
            showPannel.SetActive(true);
            displayedVictory = true;
        }
        if(showPannel.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.V))
                showPannel.SetActive(false);
        }
    }
}
