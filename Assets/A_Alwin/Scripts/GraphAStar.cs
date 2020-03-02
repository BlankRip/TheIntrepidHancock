using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript { 


[System.Serializable] public class Node
{
        public Vector3 position;
        public Node[] neighbours;
        public float gCost = 0;
        public float hCost = 0;
        public Node parentNode;

        public float fCost 
        {
            get { return gCost + hCost; }
        }

    }
public class GraphAStar : MonoBehaviour
{
   
    public static Node[] GenerateRoute(Node[] nodeList, Vector3 userPosition, Node endNode)
    {
            // check list
            List<Node> checkList = new List<Node>();
            // done list
            List<Node> doneList = new List<Node>();

            // find the closest node to where you are standig
            float bestDistance = Mathf.Infinity;
            // closest node
            Node closestNode = null;
            for (int i = 0; i < nodeList.Length; i++)
            {
                    float newDistance = Vector3.SqrMagnitude(nodeList[i].position - userPosition);
                    if (newDistance < bestDistance) 
                    {
                        closestNode = nodeList[i];
                        bestDistance = newDistance;
                    }
            }

            closestNode.hCost = Vector3.SqrMagnitude(closestNode.position - endNode.position);
            checkList.Add(closestNode);
            int breakCount = 800;
            Node lowestFNode = null;

            // run till find the end node
            while (lowestFNode != endNode)
            {
                float lowestFYet = Mathf.Infinity;
                // find the node with the lowest f cost
                for (int i = 0; i < checkList.Count; i++)
                {
                    if (checkList[i].fCost < lowestFYet) 
                    {
                        lowestFYet = checkList[i].fCost;
                        lowestFNode = checkList[i];
                    }
                }
                    // find all its neibours and claculate
                    // both h and g cost

                    for (int n = 0; n < lowestFNode.neighbours.Length; n++)
                    {
                        lowestFNode.neighbours[n].gCost = Vector3.SqrMagnitude(lowestFNode.position - lowestFNode.neighbours[n].position);
                        lowestFNode.neighbours[n].hCost = Vector3.SqrMagnitude(endNode.position - lowestFNode.neighbours[n].position);
                        lowestFNode.neighbours[n].parentNode = lowestFNode;

                        // add neibour node to the next check list if its not already checked
                        if (!doneList.Contains(lowestFNode.neighbours[n])) checkList.Add(lowestFNode.neighbours[n]);

                    }

                    // add and remove the selected node
                    doneList.Add(lowestFNode);
                    checkList.Remove(lowestFNode);
            }

            List<Node> routeList = new List<Node>();
            Node addNode = lowestFNode;
            while (addNode != closestNode) 
            {
                routeList.Add(addNode);
                addNode = lowestFNode.parentNode;
            }
            routeList.Add(addNode);


            return routeList.ToArray();

    }
    }
}
