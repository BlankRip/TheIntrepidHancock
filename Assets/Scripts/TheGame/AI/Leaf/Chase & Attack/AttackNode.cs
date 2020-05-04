using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : TreeNode
{
    float clipLength = 5f;

    public override void Run(TheAI ai)
    {
        if (!ai.recentlyAttcked)
        {
            if (status != ReturnResult.Running)
            {
                ai.myAnimator.SetTrigger("Attack");
                clipLength = ai.attackDuration;
            }

            if (clipLength <= 0)
            {
                ai.recentlyAttcked = true;
                ai.attacking = false;
                status = ReturnResult.Success;
                return;
            }

            clipLength -= Time.deltaTime;
            status = ReturnResult.Running;
        }
        else
            status = ReturnResult.Success;
    }
}
