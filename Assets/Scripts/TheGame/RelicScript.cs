using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicScript : MonoBehaviour
{
    [SerializeField] Transform ePos;
    GameObject pickUpUI;
    bool canPickup;

    private void Start()
    {
        pickUpUI = GameManager.instance.eUIobj;
    }

    private void Update()
    {
        if(canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("<color=blue> Relic Collected</color>");
            pickUpUI.SetActive(false);
            GameManager.instance.SpawnEnemy();
            GameManager.instance.ReadyToExit();
            GameManager.instance.RelicUiUpdate();
            AudioManger.instance.RelicCollectedClip();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If player collides with the relic then he collects it and the relics collected is increased by one on the tracker script
        if(other.tag == "Player")
        {
            pickUpUI.transform.position = ePos.position;
            GameManager.instance.eResetUI = true;
            pickUpUI.SetActive(true);
            canPickup = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            pickUpUI.SetActive(true);
            canPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canPickup = false;
        pickUpUI.SetActive(false);
    }

}
