using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    [HideInInspector] public int currentScore;          //The current score of the player
    [HideInInspector] public bool spawnRelic;           //If it's time to spawn a relic or not

    int spawnAtScoreMultiplier;                         //The multiplier value used in our equation to find when to spawn next relic
    int x;
    int y;
    bool exitStatusUpdate;                              //Checks if have to update the player that he can leave

    void Start()
    {
        currentScore = 0;
        x = 10;
        y = 5;
        spawnAtScoreMultiplier = 18 * x + y;
        //spawnAtScoreMultiplier = 0;
        spawnRelic = false;
        exitStatusUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Only checks if the relic must be sawned if the number of relics collected is less than 3
        if (GameManager.instance.relicsSpawned < 3)
        {
            //The equation that decideds when to spawn the relic
            if (currentScore > spawnAtScoreMultiplier)
            {
                spawnRelic = true;
                x = x * 2;
                y = Random.Range(2, 5);
                spawnAtScoreMultiplier = 18 * (x + y);
                //spawnAtScoreMultiplier++;
            }
        }
    }
}
