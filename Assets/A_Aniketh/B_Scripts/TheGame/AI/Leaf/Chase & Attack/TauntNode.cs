using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntNode : TreeNode
{
    float tauntLength = 5f;
    public override void Run(TheAI ai)
    {
        Debug.Log("<color=yellow> IN TAUNT </color>");
        if (status != ReturnResult.Running)
        {
            ai.myAnimator.SetTrigger("Taunt");
            Debug.Log("play taunting audio clip");
            tauntLength = ai.myAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        if (tauntLength <= 0)
        {
            Debug.Log("<color=yellow> Finish TAUNT </color>");
            ai.recentlyAttcked = false;
            status = ReturnResult.Success;
            return;
        }
        tauntLength -= Time.deltaTime;
        status = ReturnResult.Running;
    }
}
