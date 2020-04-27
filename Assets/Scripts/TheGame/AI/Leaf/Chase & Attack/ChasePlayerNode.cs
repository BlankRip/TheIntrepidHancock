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
        if (!ai.attacking && !ai.recentlyAttcked)
        {
            if(!ai.playingChase)
            {
                AudioManger.instance.SwitchToChase();
                ai.playingChase = true;
            }

            if (Vector3.Distance(ai.transform.position, ai.target.transform.position) < ai.attackRange)
            {
                ai.myAnimator.SetBool("Run", false);
                ai.attacking = true;
                status = ReturnResult.Success;
                return;
            }

            ai.myAnimator.SetBool("Run", true);
            ai.myAnimator.SetBool("Walk", false);
            status = ReturnResult.Running;
            ai.pathPointeReset = true;
            runAnim = true;
            collisionAvoidance = ai.CollisionAvoidance();
            ai.maxVelocity = ai.maxChaseVel;
            steering = ai.Pursuit(ai.target.transform.position, ai.targerRb, slowRadios, 16);
            ai.rb.velocity += (steering + collisionAvoidance);

            ai.targetPoint = ai.target.transform.position;
        }
    }
}
