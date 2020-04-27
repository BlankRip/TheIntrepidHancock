using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class SendObjectRefrence : UnityEvent<GameObject>{}

    public static GameManager instance;

    [HideInInspector] public bool paused;
    [HideInInspector] public int relicsSpawned;
    [SerializeField] GameObject eventObject;
    [SerializeField] GameObject exitLight;
    [SerializeField] Collider exitTriggerCollider;
    [SerializeField] GameObject[] relicPieces;
    [SerializeField] AudioSource relciCollectionSound;
    public SendObjectRefrence OnCollectRelic;


    int relicPieceTracker;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        paused = false;
        relicsSpawned = 0;
        relicPieceTracker = 0;
    }


    public void ReadyToExit()
    {
        if (relicsSpawned >= 3)
        {
            exitLight.SetActive(true);
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

    public void RelicUiUpdate()
    {
        if (relicPieceTracker < relicPieces.Length)
        {
            relicPieces[relicPieceTracker].SetActive(true);
            OnCollectRelic.Invoke(relicPieces[relicPieceTracker]);
            relciCollectionSound.Play();
            relicPieceTracker++;
        }
    }

    IEnumerator StartTheEvent()
    {
        yield return new WaitForSeconds(4);
        eventObject.SetActive(true);
        Camera.main.gameObject.SetActive(false);
    }
}
