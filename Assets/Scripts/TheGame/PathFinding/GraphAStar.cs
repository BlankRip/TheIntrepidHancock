using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlwinScript { 


[System.Serializable]
    public class Node
{
        public Vector3 position;
        public int nodeIndex = -1;
        public int[] neighbours;
        public float gCost = 0;
        public float hCost = 0;
        
        public string name;
        public float fCost 
        {
            get { return gCost + hCost; }
        }

        public Node (Vector3 pos, string nodeName, int index)
        {
            position = pos;
            name = nodeName;
            nodeIndex = index;
        }
    }
public class GraphAStar : MonoBehaviour
{
   
   public static Node startNode;
   public static LayerMask rayMask;
    public static string closestName, furthestName;

    public static Vector3[] GenerateRoute(Node[] nodeList, Vector3 userPosition, Vector3 endPosition)
    {
        closestName = "Start Node";
        furthestName = "End Node";
        try{
       
            
            // node parent recorder
            // start node
            int[] objectParentID = new int[nodeList.Length + 1];


            // check list
            List<Node> checkList = new List<Node>();
            // done list
            List<Node> doneList = new List<Node>();

            // find the closest node to where you are standig

            // set the start node to point of the player
            Node closestNode = FindClosest(nodeList, userPosition);
            if(closestNode == null) return null;
            closestName = closestNode.name;
       //     startNode = new Node(userPosition + Vector3.up, "startPoint", 0);
      //      startNode.nodeIndex = 0;
            //new Node(userPosition + Vector3.up, "startPoint", 0);
    //        SetupStartNode(nodeList, startNode);
    //        closestNode = startNode;
            // find the end node
            Node endNode = FindClosest(nodeList, endPosition);
            if(endNode == null) return null;
            furthestName = endNode.name;


     //       objectParentID[closestNode.nodeIndex] = -1;
            try
            {
                closestNode.hCost = Vector3.SqrMagnitude(closestNode.position - endNode.position);
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
            
            checkList.Add(closestNode);

            Node lowestFNode = null;

            int brekProof = 50;

            // run till find the end node
            while (brekProof != 0 && lowestFNode != endNode)
            {
                brekProof--;

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
                        if (!doneList.Contains(nodeList[lowestFNode.neighbours[n]]))
                        {
                            nodeList[lowestFNode.neighbours[n]].gCost = Vector3.SqrMagnitude(lowestFNode.position - nodeList[lowestFNode.neighbours[n]].position);
                            nodeList[lowestFNode.neighbours[n]].hCost = Vector3.SqrMagnitude(endNode.position - nodeList[lowestFNode.neighbours[n]].position);
                            objectParentID[nodeList[lowestFNode.neighbours[n]].nodeIndex] = lowestFNode.nodeIndex;
                            checkList.Add(nodeList[lowestFNode.neighbours[n]]);
                        }
                    }
                    // add and remove the selected node
                    doneList.Add(lowestFNode);
                    checkList.Remove(lowestFNode);
            }
            Debug.Log("Last node found");

            List<Vector3> routeList = new List<Vector3>();
            Node addNode = lowestFNode;

            brekProof = 50;
            routeList.Add(endPosition);

            while (brekProof != 0 && addNode != closestNode) 
            {
                brekProof--;
                /*
                try{routeList.Add(addNode.position);}
                catch(System.Exception e)
                {
                    Debug.LogError(e + "-" + addNode.name);
                }
                */
                routeList.Add(addNode.position);
                addNode = nodeList[objectParentID[addNode.nodeIndex]];
            }
            routeList.Add(addNode.position);
            routeList.Reverse();

            return routeList.ToArray();
        }
        catch(System.Exception e)
        {
            Debug.LogError("Msg from A Star : "+ " | " + closestName + " | " + furthestName + " | " + userPosition + " | " + endPosition + e );
            Debug.Break();
            return null;
        }
    }

        static Node FindClosest(Node[] nodeList, Vector3 spot)
        {
            float bestDistance = Mathf.Infinity;
            Node closestNode = null;

            for (int i = 0; i < nodeList.Length; i++)
            {
                Vector3 dir = (nodeList[i].position - spot);
                float lineDistance = dir.magnitude;
                if (lineDistance < bestDistance && !Physics.Raycast(spot, dir, lineDistance, rayMask))
                {
                    closestNode = nodeList[i];
                    bestDistance = lineDistance;
                }
            }
            return closestNode;
        }
/*
         static void SetupStartNode(Node[] nodeList, Node node)
        {
            List<int> friendNodes = new List<int>();

            for (int i = 0; i < nodeList.Length; i++)
            {
                Vector3 dir = (nodeList[i].position - node.position);
                float lineDistance = dir.magnitude;
                if (!Physics.Raycast(node.position, dir, lineDistance))
                {
                    friendNodes.Add(i);
                }
            }
            node.neighbours = friendNodes.ToArray();
        }
*/
    }
}
