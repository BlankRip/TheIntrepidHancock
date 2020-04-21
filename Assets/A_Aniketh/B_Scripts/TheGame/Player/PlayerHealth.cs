using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int hitTracker;

    [SerializeField] GameObject threeBars;
    [SerializeField] GameObject twoBars;
    [SerializeField] GameObject oneBar;

    // Start is called before the first frame update
    void Start()
    {
        hitTracker = 0;
    }

    public void ReduceHealth()
    {
        hitTracker++;

        if(hitTracker == 1)
        {
            twoBars.SetActive(true);
            threeBars.SetActive(false);
        }
        else if (hitTracker == 2)
        {
            oneBar.SetActive(true);
            twoBars.SetActive(false);
        }
        else if (hitTracker == 3)
        {
            SceneShifter.LoadScene("GameOver");
        }
    }
}
