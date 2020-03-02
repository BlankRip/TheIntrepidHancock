using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlwinScript;

public class AStarTester : MonoBehaviour
{

    void Start()
    {
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("A*Node");
        NodePoint.allNodes = allNodes;

        foreach (GameObject item in allNodes)
        {
            item.GetComponent<NodePoint>().FindFriends();
        }
    }


}
