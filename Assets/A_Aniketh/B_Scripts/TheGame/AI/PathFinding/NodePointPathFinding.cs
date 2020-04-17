using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlwinScript;

public class NodePointPathFinding : MonoBehaviour
{
    [HideInInspector] public static GameObject[] allNodes;
    [HideInInspector] public Node thisNode;

    // go through all the nodes as
    public void FindFriends()
    {
        thisNode.position = transform.position;
        List<Node> neibourNodes = new List<Node>();
        
        for (int i = 0; i < allNodes.Length; i++)
        {
            // every other node
            if (allNodes[i] != gameObject)
            {
                Vector3 dirVec = allNodes[i].transform.position - transform.position;
                if (!Physics.Raycast(transform.position, dirVec, dirVec.magnitude))
                {
                    neibourNodes.Add(allNodes[i].GetComponent<NodePointPathFinding>().thisNode);
                }
            }
        }
        // add in to neibour nodes
        thisNode.neighbours = neibourNodes.ToArray();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < thisNode.neighbours.Length; i++)
        {
            Gizmos.DrawLine(transform.position, thisNode.neighbours[i].position);
        }
    }

}
