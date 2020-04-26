using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoundCheckNode : TreeNode
{
    public override void Run(TheAI ai)
    {
        if (ai.playerFound || ai.attacking || ai.recentlyAttcked)
        {
            status = ReturnResult.Success;
        }
        else
        {
            status = ReturnResult.Fail;
        }
    }
}
