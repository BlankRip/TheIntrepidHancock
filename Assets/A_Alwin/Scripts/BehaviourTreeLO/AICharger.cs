using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{
    public class AICharger : AINode
    {
        public override AIState Run(AIBlackboard ai)
        {
            ai.compareThrushold = 95;
            ai.batteryCharge += Time.deltaTime * 5;

            if (ai.compareThrushold < 95)
            {
                ai.compareThrushold = 95;
            }

            if (ai.batteryCharge > 95)
            {
                ai.compareThrushold = 10;
            }

            return AIState.Success;
        }
    }
}