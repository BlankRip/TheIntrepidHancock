using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : TreeNode
{
    float idleEndTime = 7;
    float idleTimeTracker = 7;

    public override void Run(TheAI ai)
    {
        Debug.Log("<color=blue> IN IDLE NODE  </color>");
        if (idleTimeTracker <= 0)
        {
            Debug.Log("<color=blue>Enemy Idleing anim bool to false</color>");
            idleTimeTracker = idleEndTime;
            status = ReturnResult.Success;
            return;
        }

        Debug.Log("<color=blue>Enemy Idleing anim bool to true</color>");
        idleTimeTracker -= Time.deltaTime;
        status = ReturnResult.Running;
    }
}
