using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoundCheckNode : TreeNode
{
    public override void Run(TheAI ai)
    {
        if (ai.playerFound)
        {
            Debug.Log("<color=blue> PLAYER FOUND  </color>");
            status = ReturnResult.Fail;
        }
        else
        {
            Debug.Log("<color=yellow> PLAYER NOT FOUND  </color>");
            status = ReturnResult.Success;
        }
    }
}
