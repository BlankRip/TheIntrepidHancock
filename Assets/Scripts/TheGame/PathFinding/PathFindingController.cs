using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlwinScript;

public class PathFindingController : MonoBehaviour
{
    public GameObject[] targetSpots;

    public static PathFindingController instance;

    public Node[] allNodes;

    public Vector3[] routeNodes;

    public LayerMask rayMask;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GraphAStar.rayMask = rayMask;
        targetSpots = GameObject.FindGameObjectsWithTag("GoalPoints");
        GameObject[] gameNodes = GameObject.FindGameObjectsWithTag("A*Node");

        allNodes = new Node[gameNodes.Length];

     //   NodePointPathFinding.allNodes = allNodes;

        for (int i = 0; i < gameNodes.Length; i++)
        {
            allNodes[i] = new Node(gameNodes[i].transform.position, gameNodes[i].name,  i);
            List<int> neibourIndex =  new List<int>();
            for (int n = 0; n < gameNodes.Length; n++)
            {
                if(i!=n)
                {
                    Vector3 dir = gameNodes[n].transform.position - allNodes[i].position;
                    if(!Physics.Raycast(allNodes[i].position, dir.normalized, dir.magnitude, rayMask))
                    {
                        neibourIndex.Add(n);
                    }
                }
            }
            allNodes[i].neighbours = neibourIndex.ToArray();
        }


/*
        foreach (GameObject item in allNodes)
        {
            NodePointPathFinding pickNodeObject = item.GetComponent<NodePointPathFinding>();
            pickNodeObject.thisNode.name = item.name;
            pickNodeObject.thisNode.nodeIndex = index;
            nodeList.Add(pickNodeObject.thisNode);
            pickNodeObject.FindFriends();
            index++;
        }
*/
    //    this.allNodes = nodeList.ToArray();
    }

    public Vector3[] GetRandomGoalRout(Transform startPoint)
    {
        bool pickFarGoal = false;
        Transform endPoint = null;

        while(!pickFarGoal)
        {
            endPoint = targetSpots[Random.Range(0, targetSpots.Length)].transform;
            Vector3 dir = endPoint.position - startPoint.position;
            if(Physics.Raycast(startPoint.position,dir.normalized, dir.magnitude))
            {
                pickFarGoal = true;
            }
        }

        return GraphAStar.GenerateRoute(allNodes, startPoint.position, endPoint.position);
    }



    public Vector3[] GetRandomGoalRout(Transform startPoint, Vector3 goal)
    {
        return GraphAStar.GenerateRoute(allNodes, startPoint.position, goal);
    }
}
