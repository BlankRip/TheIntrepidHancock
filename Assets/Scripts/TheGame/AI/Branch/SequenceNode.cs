using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : TreeNode
{
    public override void Run(TheAI ai)
    {
        for (int i = 0; i < refToChildren.Count; i++)
        {
            refToChildren[i].Run(ai);
            if(refToChildren[i].status == ReturnResult.Running || refToChildren[i].status == ReturnResult.Fail)
            {
                status = refToChildren[i].status;
                return;
            }
        }
        status = ReturnResult.Success;
    }
}
