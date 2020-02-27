using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool attacking;
    public bool paused;
    public int relicsCollected;
    Collider exitTriggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        paused = false;
        relicsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
