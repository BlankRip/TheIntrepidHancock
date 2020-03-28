using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroleNode : TreeNode
{
    Vector3[] pathNodes;
    Vector3 steering;
    Vector3 collisionAvoidense;
    int currentNodeIndex;

    Vector3 targetPoint;

    public override void Run(TheAI ai)
    {
        Debug.Log("<color=blue> IN PATROL NODE  </color>");

        if (ai.pathPointeReset)
        {
            Debug.Log("<color=cyan> resetting </color>");
            pathNodes = null;
            ai.pathPointeReset = false;
        }


        if (pathNodes == null)
        {
            pathNodes = PathFindingController.instance.GetRandomGoalRout(ai.transform);
            status = ReturnResult.Running;
            currentNodeIndex = 0;
        }
        else
        {
            if (Vector3.Distance(ai.transform.position, pathNodes[currentNodeIndex]) < ai.reachRegisterDistance)
            {
                if (currentNodeIndex == pathNodes.Length - 1)
                {
                    ai.myAnimator.SetBool("Walk", false);
                    status = ReturnResult.Success;
                    currentNodeIndex = 0;
                    pathNodes = null;
                    return;
                }
                else
                    currentNodeIndex++;
            }

            status = ReturnResult.Running;
            ai.myAnimator.SetBool("Walk", true);
            ai.myAnimator.SetBool("Run", false);
            collisionAvoidense = ai.CollisionAvoidance();
            steering = ai.Seek(pathNodes[currentNodeIndex], 0.8f);
            ai.rb.velocity += (steering + collisionAvoidense);
            ai.targetPoint = pathNodes[currentNodeIndex];
        }
    }



}
