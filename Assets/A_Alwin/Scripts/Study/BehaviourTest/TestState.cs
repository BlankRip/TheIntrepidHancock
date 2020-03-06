using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestState
{
    public TestState parentState;
    public abstract TestBehaviour.StateCases Run();

}
