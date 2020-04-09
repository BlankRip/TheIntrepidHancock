using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{

    public class AIReachTargetPoint : AINode
    {
        public override AIState Run(AIBlackboard ai)
        {
            Vector3 dist = ai.reachPoint - ai.transform.position;
            if (dist.magnitude > 1)
            {
                Vector3 diffvector = (dist.normalized - ai.velocity);
                ai.velocity += (diffvector * ai.steeringFactor);
                ai.transform.Translate(ai.velocity * Time.deltaTime * 5);
                ai.batteryCharge -= Time.deltaTime * 5;
                return AIState.Running;
            }
            else
            {
                return AIState.Success;
            }
        }
    }
}