using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroleNode : TreeNode
{
    Vector3[] pathNodes;
    Vector3 steering;
    Vector3 collisionAvoidense;
    int currentNodeIndex;

    public override void Run(TheAI ai)
    {
        Debug.Log("<color=blue> IN PATROL NODE  </color>");
        if (pathNodes == null)
        {
            pathNodes = PathFindingController.instance.GetRandomGoalRout(ai.transform);
            currentNodeIndex = 0;
        }
        else
        {
            if (Vector3.Distance(ai.transform.position, pathNodes[currentNodeIndex]) < ai.reachRegisterDistance)
            {
                if (currentNodeIndex == pathNodes.Length - 1)
                {
                    status = ReturnResult.Success;
                    currentNodeIndex = 0;
                    pathNodes = null;
                    return;
                }
                else
                    currentNodeIndex++;
            }

            status = ReturnResult.Running;
            collisionAvoidense = ai.CollisionAvoidance();
            steering = ai.Seek(pathNodes[currentNodeIndex], 0.8f);
            ai.rb.velocity += (steering + collisionAvoidense);
        }
    }
}
