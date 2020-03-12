using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollerPE : MonoBehaviour
{
    public GameObject cam;
    int index = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
        }
        if (index == 1)
        {
            cam.transform.position = new Vector3(-12.7f, 6.5f, 10.5f);
        }
        if (index == 2)
        {
            cam.transform.position = new Vector3(-4.25f, 6.5f, 10.5f);
        }
        if (index == 3)
        {
            cam.transform.position = new Vector3(3.7f, 6.5f, 10.5f);
        }
        if (index == 4)
        {
            cam.transform.position = new Vector3(13.5f, 6.5f, 10.5f);
        }

        if(index < 1)
        {
            index = 1;
        }

        if (index > 4)
        {
            index = 4;
        }

    }
}
