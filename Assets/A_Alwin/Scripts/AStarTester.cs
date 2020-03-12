using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlwinScript;

public class AStarTester : MonoBehaviour
{
    public Node[] allNodes;

    public Vector3[] routeNodes;

    public Transform startPoint, endPoint;

    void Start()
    {
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("A*Node");
        NodePoint.allNodes = allNodes;
        List<Node> nodeList = new List<Node>();
        int index = 0;
        foreach (GameObject item in allNodes)
        {
            NodePoint pickNodeObject = item.GetComponent<NodePoint>();
            pickNodeObject.thisNode.name = item.name;
            pickNodeObject.thisNode.nodeIndex = index;
            nodeList.Add(pickNodeObject.thisNode);
            pickNodeObject.FindFriends();
            index++;
        }

        this.allNodes = nodeList.ToArray();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            routeNodes = GraphAStar.GenerateRoute(allNodes, startPoint.position, endPoint.position);
            Debug.Log("Path created");
        }
    }

    private void OnDrawGizmos()
    {
        if (routeNodes != null && routeNodes.Length > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 1; i < routeNodes.Length; i++)
            {
                Gizmos.DrawLine(routeNodes[i - 1], routeNodes[i]);
            }
        }
    }

}
