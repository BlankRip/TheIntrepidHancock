using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownNode : TreeNode
{
    float coolDownTimer = 2;
    float coolDownTracker = 2;

    public override void Run(TheAI ai)
    {
        if (coolDownTracker <= 0)
        {
            coolDownTracker = coolDownTimer;
            ai.recentlyAttcked = false;
            status = ReturnResult.Success;
            return;
        }

        coolDownTracker -= Time.deltaTime * ai.coolDownSpeed;
        status = ReturnResult.Running;
    }
}
