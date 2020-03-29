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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        targetSpots = GameObject.FindGameObjectsWithTag("GoalPoints");
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("A*Node");
        NodePointPathFinding.allNodes = allNodes;
        List<Node> nodeList = new List<Node>();
        int index = 0;

        foreach (GameObject item in allNodes)
        {
            NodePointPathFinding pickNodeObject = item.GetComponent<NodePointPathFinding>();
            pickNodeObject.thisNode.name = item.name;
            pickNodeObject.thisNode.nodeIndex = index;
            nodeList.Add(pickNodeObject.thisNode);
            pickNodeObject.FindFriends();
            index++;
        }

        this.allNodes = nodeList.ToArray();
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
