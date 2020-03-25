using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeIdlekNode : TreeNode
{
    float idleEndTime;
    bool animationStarted;
    public override void Run(TheAI ai)
    {
        if (idleEndTime <= Time.time)
        {
            if (!animationStarted)
            {
                idleEndTime = Time.time + 10;
                Debug.Log("Enemy Idle Started");
                animationStarted = true;
            }
            else
            {
                animationStarted = false;
                Debug.Log("Enemy Idle Started");
                status = ReturnResult.Success;
            }
        }

        if (idleEndTime > Time.time)
        {
            Debug.Log("Enemy Idle running");
            status = ReturnResult.Running;
        }

    }
}
