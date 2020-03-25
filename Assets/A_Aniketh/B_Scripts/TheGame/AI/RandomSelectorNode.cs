using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelectorNode : TreeNode
{
    bool pickedNode = true;
    int chooser;
    public override void Run(TheAI ai)
    {
        if (pickedNode)
            chooser = Random.Range(0, refToChildren.Count);

        refToChildren[chooser].Run(ai);
        status = refToChildren[chooser].status;

        if (status == ReturnResult.Running)
            pickedNode = false;
        else
            pickedNode = true;
    }
}
