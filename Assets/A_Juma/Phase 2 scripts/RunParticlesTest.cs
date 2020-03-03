using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunParticlesTest : MonoBehaviour
{
    public float timer = 0;
    public int mode;
    public float resetTimer;
    public float backnforth;
    public int speed;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (mode == 1)
        {
            if (timer < backnforth)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
            }
            if (timer > backnforth)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
            }
            if (timer >= resetTimer)
            {
                timer = 0;
            }
        }

        if(mode == 2)
        {
            if (timer < backnforth)
            {
                transform.Translate(0,0, -speed * Time.deltaTime);
            }
            if (timer > backnforth)
            {
                transform.Translate(0,0, speed * Time.deltaTime);
            }
            if (timer >= resetTimer)
            {
                timer = 0;
            }
        }

    }
}
