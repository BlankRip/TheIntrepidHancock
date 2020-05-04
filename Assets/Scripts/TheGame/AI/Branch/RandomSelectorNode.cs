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
            chooser = PickRandomChild(ai.randomWeightageAdjuster);

        refToChildren[chooser].Run(ai);
        status = refToChildren[chooser].status;

        if (status == ReturnResult.Running)
            pickedNode = false;
        else
            pickedNode = true;
    }

    int PickRandomChild(float weightageAdjuster)
    {
        float pickWithWeight = Random.Range(0, 100);
        pickWithWeight = pickWithWeight / 100;
        pickWithWeight = Mathf.Pow(pickWithWeight, weightageAdjuster) * refToChildren.Count;

        int pick = (int)pickWithWeight;

        return pick;
    }
}
