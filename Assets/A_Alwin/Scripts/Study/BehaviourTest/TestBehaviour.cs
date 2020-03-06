using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestBehaviour : MonoBehaviour
{
    public enum StateCases
    {
        running,
        done,
        failed,
    }

    public TestState currentState;



    void Start()
    {
        
    }

 
    void Update()
    {
        if (currentState != null)
        {
            StateCases returnCase = currentState.Run();

            if (returnCase == StateCases.running) return;

            else if (returnCase == StateCases.done)
            {

            }

            else if (returnCase == StateCases.failed)
            {

            }
        }
    }
}
