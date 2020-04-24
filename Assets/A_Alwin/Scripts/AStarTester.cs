using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlwinScript;

public class AStarTester : MonoBehaviour
{
    
    public GameObject[] targetSpots;

    public Node[] allNodes;

    public Vector3[] routeNodes;

    public Transform startPoint, endPoint;

    public LayerMask rayMask;


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
        /*
        if(GraphAStar.startNode != null)
        {
            foreach (Node item in GraphAStar.startNode.neighbours)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(GraphAStar.startNode.position, item.position);
                
            }
        }
        */
    }

}
