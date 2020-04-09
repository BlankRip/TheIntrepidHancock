using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{

    public class AICheckCharge : AINode
    {
        public override AIState Run(AIBlackboard ai)
        {
            if (ai.batteryCharge < ai.compareThrushold) {
                ai.reachPoint = ai.rechargePoint.position;
                return AIState.Success;
            } 
            else return AIState.Fail;
        }
    }
}