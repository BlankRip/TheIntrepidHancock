using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBotCharge : MoveBotState
{
    public override void ChangeState()
    {
        botObject.currnt = new MoveBotPatrole();
        botObject.currnt.AssignBot(botObject);
    }

    public override void Execute()
    {
        botObject.rechargeThrushold = 100;

        float distance = (botObject.chargePoint.position - botObject.transform.position).magnitude;

        if (distance > 1)
        {
            botObject.targetLocation = botObject.chargePoint;
            MoveTo();
        }
        else
        {
            botObject.charge += 10 * Time.deltaTime;

            if (botObject.charge > 95)
            {
                botObject.rechargeThrushold = 10;
                ChangeState();
            }
        }
    }


}
