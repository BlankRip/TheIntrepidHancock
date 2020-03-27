using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : TreeNode
{
    float clipLength;

    public override void Run(TheAI ai)
    {
        Debug.Log("<color=red> IN ATTACK </color>");
        if (status != ReturnResult.Running)
        {
            Debug.Log("<color=red>Enemy attack anim triggered</color>");
            Debug.Log("<color=red>Enemy attack sound</color>");
            clipLength = ai.myAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        if(clipLength <=0)
        {
            status = ReturnResult.Success;
            return;
        }

        clipLength -= Time.deltaTime;
        status = ReturnResult.Running;
    }
}
