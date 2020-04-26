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
            ai.setSearchCount = true;
            status = ReturnResult.Fail;
        }
    }
}
