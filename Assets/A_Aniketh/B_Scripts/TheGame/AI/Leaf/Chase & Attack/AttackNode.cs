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
            Debug.Log("<color=red> IN ATTACK </color>");
            if (status != ReturnResult.Running)
            {
                ai.myAnimator.SetTrigger("Attack");
                Debug.Log("<color=red>Enemy attack sound</color>");
                clipLength = ai.myAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            }

            if (clipLength <= 0)
            {
                Debug.Log("<color=red> ATTACK Success </color>");
                ai.recentlyAttcked = true;
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
