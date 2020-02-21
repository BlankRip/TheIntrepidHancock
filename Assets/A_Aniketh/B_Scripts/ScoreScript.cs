using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    int spawnAtScoreMultiplier;                         //The multiplier value used in our equation to find when to spawn next relic
    [HideInInspector] public int currentScore;          //The current score of the player
    [HideInInspector] public int relicsCollected;       //The number of relics collected so far
    [HideInInspector] public bool spawnRelic;           //If it's time to spawn a relic or not
    bool exitStatusUpdate;                              //Checks if have to update the player that he can leave
    [SerializeField] Collider exitTriggerCollider;      //The is trigger collider on the Exit-Door

    void Start()
    {
        currentScore = 0;
        relicsCollected = 0;
        spawnAtScoreMultiplier = 1;
        spawnRelic = false;
        exitStatusUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Only checks if the relic must be sawned if the number of relics collected is less than 3
        if (relicsCollected < 3)
        {
            //The equation that decideds when to spawn the relic
            if (currentScore > 9 * spawnAtScoreMultiplier)
            {
                spawnRelic = true;
                spawnAtScoreMultiplier++;
            }
        }
        else if (relicsCollected >= 3 && !exitStatusUpdate)
        {
            Debug.Log("<color=red>Now play the voiceLine and turn on exit trigger collider");
            exitStatusUpdate = true;
        }
    }
}
