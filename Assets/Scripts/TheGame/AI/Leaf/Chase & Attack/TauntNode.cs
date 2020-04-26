using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntNode : TreeNode
{
    float tauntLength = 5f;
    public override void Run(TheAI ai)
    {
        if (status != ReturnResult.Running)
        {
            ai.myAnimator.SetTrigger("Taunt");
            tauntLength = ai.tauntDuration;
        }

        if (tauntLength <= 0)
        {
            ai.recentlyAttcked = false;
            status = ReturnResult.Success;
            return;
        }
        tauntLength -= Time.deltaTime;
        status = ReturnResult.Running;
    }
}
