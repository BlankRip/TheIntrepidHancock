using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public bool paused;
    [HideInInspector] public int relicsSpawned;
    [SerializeField] GameObject eventObject;
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

    public void SpawnEnemy()
    {
        if (relicsSpawned == 1)
        {
            if (eventObject != null)
            {
                StartCoroutine(StartTheEvent());
            }
        }
        else if (relicsSpawned == 3)
        {
            //Spawn The Curry
        }
    }

    IEnumerator StartTheEvent()
    {
        yield return new WaitForSeconds(4);
        eventObject.SetActive(true);
        Camera.main.gameObject.SetActive(false);
    }
}
