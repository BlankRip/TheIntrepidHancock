﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNode : TreeNode
{
    int numberOfSearches;
    int countTracker;
    bool setSearchCount = true;
    bool goToPlayerPos = true;

    Vector3[] pathNodes;
    Vector3 steering;
    Vector3 collisionAvoidense;
    int currentNodeIndex;
    float searchRadios = 20;

    public override void Run(TheAI ai)
    {
        Debug.Log("<color=green> IN SEARCH NODE </color>");
        if (ai.justEscaped)
        {
            if (setSearchCount)
            {
                numberOfSearches = Random.Range(ai.minNumberOfSearches, ai.maxNumberOfSearches + 1);
                countTracker = 0;
                setSearchCount = false;
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

                    if (Vector3.Distance(ai.transform.position, pathNodes[currentNodeIndex]) < 1)
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

                    if (Vector3.Distance(ai.transform.position, pathNodes[currentNodeIndex]) < 0.5)
                    {
                        if (Vector3.Distance(ai.transform.position, ai.lastSeenPos) > searchRadios)
                        {
                            countTracker++;
                            goToPlayerPos = true;
                            currentNodeIndex = 0;
                            pathNodes = null;
                        }
                        else
                            currentNodeIndex++;
                    }

                }

                if(pathNodes != null)
                {
                    collisionAvoidense = ai.CollisionAvoidance();
                    steering = ai.Seek(pathNodes[currentNodeIndex], 0.8f);
                    ai.rb.velocity += (steering + collisionAvoidense);
                }
            }
            else
            {
                setSearchCount = true;
                ai.justEscaped = false;
                status = ReturnResult.Success;
            }
        }
        else
            status = ReturnResult.Fail;
    }
}
