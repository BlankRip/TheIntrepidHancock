using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlwinScript;

public class NodePoint : MonoBehaviour
{
    public static GameObject[] allNodes;
    public Node thisNode;

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
                    neibourNodes.Add(allNodes[i].GetComponent<NodePoint>().thisNode);
                }
            }
        }
        // add in to neibour nodes
        thisNode.neighbours = neibourNodes.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (thisNode.neighbours.Length > 0)
        {
            foreach (Node item in thisNode.neighbours)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, item.position);
            }
        }
    }

}
