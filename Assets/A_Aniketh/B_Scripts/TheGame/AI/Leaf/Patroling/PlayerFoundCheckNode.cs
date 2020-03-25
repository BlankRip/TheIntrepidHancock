using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoundCheckNode : TreeNode
{
    public override void Run(TheAI ai)
    {
        if (ai.playerFound)
            status = ReturnResult.Fail;
        else
            status = ReturnResult.Success;
    }
}
