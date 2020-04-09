using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{
    public class AISequencer : AINode
    {
        public override AIState Run(AIBlackboard ai)
        {
            foreach (AINode node in childNodes)
            {
                AIState state = node.Run(ai);
                if (state == AIState.Fail || state == AIState.Running)
                {
                    return state;
                }
            }
            return AIState.Success;
        }
    }
}