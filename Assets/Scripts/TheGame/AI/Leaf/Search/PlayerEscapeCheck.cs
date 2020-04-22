using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscapeCheck : TreeNode
{
    public override void Run(TheAI ai)
    {
        if (ai.justEscaped)
            status = ReturnResult.Success;

        if(!ai.justEscaped)
        {
            Debug.Log("<color=green> Escape check Failed</color>");
            ai.setSearchCount = true;
            status = ReturnResult.Fail;
        }
        else
            Debug.Log("<color=green> Escape check Success</color>");
    }
}
