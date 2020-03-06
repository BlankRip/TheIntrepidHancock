using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript { 


[System.Serializable]
    public class Node
{
        public Vector3 position;
        public int nodeIndex = -1;
        public Node[] neighbours;
        public float gCost = 0;
        public float hCost = 0;
        
        public string name;
        public float fCost 
        {
            get { return gCost + hCost; }
        }

    }
public class GraphAStar : MonoBehaviour
{
   
    public static Vector3[] GenerateRoute(Node[] nodeList, Vector3 userPosition, Vector3 endPosition)
    {
            // node parent recorder
            int[] objectParentID = new int[nodeList.Length];


            // check list
            List<Node> checkList = new List<Node>();
            // done list
            List<Node> doneList = new List<Node>();

            // find the closest node to where you are standig

            Node closestNode = FindClosest(nodeList, userPosition);

            // find the end node
            Node endNode = FindClosest(nodeList, endPosition);


            objectParentID[closestNode.nodeIndex] = -1;
            closestNode.hCost = Vector3.SqrMagnitude(closestNode.position - endNode.position);
            checkList.Add(closestNode);

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


                    // add neibour node to the next check list if its not already checked
                    if (!doneList.Contains(lowestFNode.neighbours[n]))
                    {
                        lowestFNode.neighbours[n].gCost = Vector3.SqrMagnitude(lowestFNode.position - lowestFNode.neighbours[n].position);
                        lowestFNode.neighbours[n].hCost = Vector3.SqrMagnitude(endNode.position - lowestFNode.neighbours[n].position);
                        objectParentID[lowestFNode.neighbours[n].nodeIndex] = lowestFNode.nodeIndex;
                        checkList.Add(lowestFNode.neighbours[n]);
                    }

                    }

                    // add and remove the selected node
                    doneList.Add(lowestFNode);
                    checkList.Remove(lowestFNode);
            }
            
            List<Vector3> routeList = new List<Vector3>();
            Node addNode = lowestFNode;

            while (addNode != closestNode) 
            {
                routeList.Add(addNode.position);
                addNode = nodeList[objectParentID[addNode.nodeIndex]];
            }
            routeList.Add(addNode.position);


            return routeList.ToArray();
    }

        static Node FindClosest(Node[] nodeList, Vector3 spot)
        {
            float bestDistance = Mathf.Infinity;
            Node closestNode = null;

            for (int i = 0; i < nodeList.Length; i++)
            {
                Vector3 dir = (nodeList[i].position - spot);
                float lineDistance = dir.magnitude;
                if (lineDistance < bestDistance && !Physics.Raycast(spot, dir, lineDistance))
                {
                    closestNode = nodeList[i];
                    bestDistance = lineDistance;
                }
            }
            return closestNode;
        }

    }
}
