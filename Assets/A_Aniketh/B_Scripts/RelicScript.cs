using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicScript : MonoBehaviour
{
    ScoreScript relicTracker;

    void Start()
    {
        relicTracker = FindObjectOfType<ScoreScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            relicTracker.relicsCollected++;
            Destroy(gameObject);
        }
    }

}
