using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertorNode : TreeNode
{
    public override void Run(TheAI ai)
    {
        for (int i = 0; i < refToChildren.Count; i++)
        {
            refToChildren[i].Run(ai);

            switch (refToChildren[i].status)
            {
                case ReturnResult.Fail:
                    {
                        status = ReturnResult.Success;
                        return;
                    }
                case ReturnResult.Success:
                    {
                        status = ReturnResult.Fail;
                        return;
                    }
            }
        }
        status = ReturnResult.Running;
    }
}
