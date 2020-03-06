using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MoveBotState 
{

    protected MoveBot botObject;

    public abstract void Execute();

    public void MoveTo()
    {
        botObject.transform.Translate((botObject.targetLocation.position - botObject.transform.position).normalized * Time.deltaTime * botObject.speed);
        botObject.charge -= botObject.chargeDrain * Time.deltaTime;
    }

    public void AssignBot(MoveBot bot)
    {
        botObject = bot;
    }

    public abstract void ChangeState();
}
