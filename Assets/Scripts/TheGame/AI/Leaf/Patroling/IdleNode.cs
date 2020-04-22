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
            idleTimeTracker = idleEndTime;
            ai.recentlyAttcked = false;
            status = ReturnResult.Success;
            return;
        }

        ai.myAnimator.SetBool("Run", false);
        ai.myAnimator.SetBool("Walk", false);
        idleTimeTracker -= Time.deltaTime;
        status = ReturnResult.Running;
    }
}
