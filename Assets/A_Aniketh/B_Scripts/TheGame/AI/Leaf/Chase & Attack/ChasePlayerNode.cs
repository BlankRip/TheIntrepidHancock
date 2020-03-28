using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerNode : TreeNode
{
    Vector3 steering;
    Vector3 collisionAvoidance;
    float slowRadios = 10;
    bool runAnim;

    public override void Run(TheAI ai)
    {
        //if(status != ReturnResult.Running)
        //{
        //}

        if (Vector3.Distance(ai.transform.position, ai.target.transform.position) < ai.attackRange)
        {
            Debug.Log("<color=red> Chase Success  </color>");
            ai.myAnimator.SetBool("Run", false);
            status = ReturnResult.Success;
            return;
        }

        Debug.Log("<color=red> IN CHASE PLAYER  </color>");
        ai.myAnimator.SetBool("Run", true);
        ai.myAnimator.SetBool("Walk", false);
        status = ReturnResult.Running;
        runAnim = true;
        collisionAvoidance = ai.CollisionAvoidance();
        steering = ai.Pursuit(ai.target.transform.position, ai.targerRb, slowRadios);
        ai.rb.velocity += (steering + collisionAvoidance);
    }
}
