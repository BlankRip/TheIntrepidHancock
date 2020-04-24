﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownNode : TreeNode
{
    float coolDownTimer = 3;
    float coolDownTracker = 3;

    public override void Run(TheAI ai)
    {
  //      Debug.Log("<color=yellow> IN COOLDOWN </color>");
        if (coolDownTracker <= 0)
        {
   //         Debug.Log("<color=yellow> FINISH COOLDOWN </color>");
            coolDownTracker = coolDownTimer;
            ai.recentlyAttcked = false;
            status = ReturnResult.Success;
            return;
        }

        coolDownTracker -= Time.deltaTime * ai.coolDownSpeed;
        status = ReturnResult.Running;
    }
}
