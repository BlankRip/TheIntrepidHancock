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


    public void ReadyToExit()
    {
        if (relicsCollected < 3)
        {
            Debug.Log("<color=green> plaay ready to leave clip and unlock exit trigger on door </color>");
        }
    }
}
