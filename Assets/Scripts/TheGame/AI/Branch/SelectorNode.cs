using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : TreeNode
{
    public override void Run(TheAI ai)
    {
        for (int i = 0; i < refToChildren.Count; i++)
        {
            refToChildren[i].Run(ai);

            if(refToChildren[i].status == ReturnResult.Success || refToChildren[i].status == ReturnResult.Running)
            {
                status = refToChildren[i].status;
                return;
            }
        }
        status = ReturnResult.Fail;
    }
}
