using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : TreeNode
{
    float idleEndTime = 7;
    float idleTimeTracker = 7;

    public override void Run(TheAI ai)
    {
        if(idleTimeTracker <= 0)
        {
            Debug.Log("Enemy Idleing anim bool to false");
            idleTimeTracker = idleEndTime;
            status = ReturnResult.Success;
            return;
        }

        Debug.Log("Enemy Idleing anim bool to true");
        idleTimeTracker -= Time.deltaTime;
        status = ReturnResult.Running;
    }
}
