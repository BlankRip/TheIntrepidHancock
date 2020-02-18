using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    int spawnAtScoreMultiplier;
    public int currentScore;
    [HideInInspector] public int relicsCollected;
    public bool spawnRelic;

    void Start()
    {
        currentScore = 0;
        relicsCollected = 0;
        spawnAtScoreMultiplier = 1;
        spawnRelic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (relicsCollected < 3)
        {
            if (currentScore > 9 * spawnAtScoreMultiplier)
            {
                spawnRelic = true;
                spawnAtScoreMultiplier++;
            }
        }
    }
}
