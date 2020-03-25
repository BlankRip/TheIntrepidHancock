using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntNode : TreeNode
{
    float tauntLength;
    public override void Run(TheAI ai)
    {
        if (status != ReturnResult.Running)
        {
            Debug.Log("play taunting animation");
            Debug.Log("play taunting audio clip");
            tauntLength = ai.myAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        if (tauntLength <= 0)
        {
            status = ReturnResult.Success;
            return;
        }
        tauntLength -= Time.deltaTime;
        status = ReturnResult.Running;
    }
}
