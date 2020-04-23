using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    [HideInInspector] public int currentScore;          //The current score of the player
    [HideInInspector] public int relicsSpawned;       //The number of relics collected so far
    [HideInInspector] public bool spawnRelic;           //If it's time to spawn a relic or not

    int spawnAtScoreMultiplier;                         //The multiplier value used in our equation to find when to spawn next relic
    int x;
    int y;
    int z;
    bool exitStatusUpdate;                              //Checks if have to update the player that he can leave

    void Start()
    {
        currentScore = 0;
        relicsSpawned = 0;
        x = 7;
        y = 0;
        spawnAtScoreMultiplier = 69 * x + y;
        spawnRelic = false;
        exitStatusUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Only checks if the relic must be sawned if the number of relics collected is less than 3
        if (relicsSpawned < 3)
        {
            //The equation that decideds when to spawn the relic
            if (currentScore > spawnAtScoreMultiplier)
            {
                spawnRelic = true;
                x = x + y;
                y = Random.Range(22, 27);
                spawnAtScoreMultiplier = 69 * (x + y);
                relicsSpawned++;
            }
        }
    }
}
