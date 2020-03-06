using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuState : TestState
{
    public override TestBehaviour.StateCases Run()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            return TestBehaviour.StateCases.done;
        }

        return TestBehaviour.StateCases.running;
    }
}
