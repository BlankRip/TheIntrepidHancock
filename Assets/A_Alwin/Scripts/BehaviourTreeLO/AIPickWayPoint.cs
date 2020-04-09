using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{

    public class AIPickWayPoint : AINode
    {
        public override AIState Run(AIBlackboard ai)
        {
            Vector3 dir = ai.reachPoint - ai.transform.position;
            if (dir.magnitude < 1)
            {
                ai.reachPoint = ai.pathPoints[Random.Range(0, ai.pathPoints.Length)].position;
            }
            return AIState.Success;
        }
    }
}