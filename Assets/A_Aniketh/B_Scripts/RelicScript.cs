using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicScript : MonoBehaviour
{
    GameManager relicTracker;      //The script that keeps track of how many relics are collected

    void Start()
    {
        relicTracker = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If player collides with the relic then he collects it and the relics collected is increased by one on the tracker script
        if(other.tag == "Player")
        {
            Debug.Log("<color=blue> Relic Collected</color>");
            relicTracker.relicsCollected++;
            Destroy(gameObject);
        }
    }

}
