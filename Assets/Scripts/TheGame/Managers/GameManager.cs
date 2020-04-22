using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public bool paused;
    [HideInInspector] public int relicsSpawned;
    [SerializeField] Collider exitTriggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        paused = false;
        relicsSpawned = 0;
    }


    public void ReadyToExit()
    {
        if (relicsSpawned < 3)
        {
            Debug.Log("<color=green> plaay ready to leave clip and unlock exit trigger on door </color>");
            exitTriggerCollider.enabled = true;
        }
    }
}
