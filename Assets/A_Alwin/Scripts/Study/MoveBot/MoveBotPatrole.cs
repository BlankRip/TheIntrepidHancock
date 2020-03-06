using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBotPatrole : MoveBotState
{
    public override void ChangeState()
    {
        botObject.currnt = new MoveBotCharge();
        botObject.currnt.AssignBot(botObject);
    }

    public override void Execute()
    {

        if (botObject.charge > botObject.rechargeThrushold)
        {

            if (botObject.targetLocation == null)
            {
                botObject.targetLocation = botObject.wayPoints[Random.Range(0, botObject.wayPoints.Length)];
            }

            float distance = (botObject.targetLocation.position - botObject.transform.position).magnitude;

            if (distance < 0.5f)
            {
                botObject.targetLocation = botObject.wayPoints[Random.Range(0, botObject.wayPoints.Length)];
            }

            else
            {
                MoveTo();
            }
        }
        else
        {
            ChangeState();
        }
    }

}
