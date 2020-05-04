using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNode : TreeNode
{
    int numberOfSearches;
    int countTracker;
    bool goToPlayerPos = true;

    Vector3[] pathNodes;
    Vector3 steering;
    Vector3 collisionAvoidense;
    int currentNodeIndex;
    float searchRadios = 30;

    public override void Run(TheAI ai)
    {
        if (ai.pathPointeReset)
        {
            pathNodes = null;
            countTracker = 0;
            ai.pathPointeReset = false;
        }

        ai.myAnimator.SetBool("Run", true);
        ai.myAnimator.SetBool("Walk", false);

        if (ai.setSearchCount)
        {
            numberOfSearches = Random.Range(ai.minNumberOfSearches, ai.maxNumberOfSearches + 1);
            countTracker = 0;
            ai.setSearchCount = false;
        }

        if (countTracker < numberOfSearches)
        {
            status = ReturnResult.Running;


            if (goToPlayerPos)
            {
                if (pathNodes == null)
                {
                    pathNodes = PathFindingController.instance.GetRandomGoalRout(ai.transform, ai.lastSeenPos);
                    currentNodeIndex = 0;
                }

                if (Vector3.Distance(ai.transform.position, pathNodes[currentNodeIndex]) < ai.reachRegisterDistance)
                {
                    if (currentNodeIndex == pathNodes.Length - 1)
                    {
                        goToPlayerPos = false;
                        currentNodeIndex = 0;
                        pathNodes = null;
                    }
                    else
                        currentNodeIndex++;
                }
            }
            else
            {
                if (pathNodes == null)
                {
                    pathNodes = PathFindingController.instance.GetRandomGoalRout(ai.transform);
                    currentNodeIndex = 0;
                }

                if (Vector3.Distance(ai.transform.position, pathNodes[currentNodeIndex]) < ai.reachRegisterDistance)
                {
                    if (Vector3.Distance(ai.transform.position, ai.lastSeenPos) > searchRadios || currentNodeIndex == pathNodes.Length - 1)
                    {
                        countTracker++;
                        goToPlayerPos = true;
                        currentNodeIndex = 0;
                        pathNodes = null;
                    }
                    else if (currentNodeIndex < pathNodes.Length - 1)
                        currentNodeIndex++;
                }

            }

            if (pathNodes != null)
            {
                collisionAvoidense = ai.CollisionAvoidance();
                steering = ai.Seek(pathNodes[currentNodeIndex], 0.8f);
                ai.rb.velocity += (steering + collisionAvoidense);
                ai.targetPoint = pathNodes[currentNodeIndex];
            }
        }
        else
        {
            ai.setSearchCount = true;
            ai.justEscaped = false;
            ai.myAnimator.SetBool("Run", false);
            ai.pathPointeReset = true;
            status = ReturnResult.Success;
        }
    }
}
