using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript.AI
{
    public enum AIState{Fail, Success, Running}
    public abstract class AINode
    {
        public List<AINode> childNodes = new List<AINode>();
        public abstract AIState Run(AIBlackboard ai);
    }
}