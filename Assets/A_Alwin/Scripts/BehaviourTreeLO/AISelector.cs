using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{
    public class AISelector : AINode
    {
        public override AIState Run(AIBlackboard ai)
        {
            foreach (AINode node in childNodes)
            {
                AIState state = node.Run(ai);
                if (state == AIState.Success || state == AIState.Running)
                {
                    return state;
                }
            }
            return AIState.Fail;
        }
    }
}