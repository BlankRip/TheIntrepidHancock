﻿using System.Collections;
using System.Collections.Generic;
using System.Media;
using UnityEngine;

public class ScriptedEvent : MonoBehaviour
{
    [SerializeField] float eventLength;
    [SerializeField] GameObject eventCamera;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject enemy2;
    [SerializeField] AudioSource eventSource;

    // Start is called before the first frame update
    void Start()
    {
        AudioManger.instance.AbbyArrival(eventSource);
        AudioManger.instance.playBreakAudio = false;
        StartCoroutine(TheEvent());
    }

    IEnumerator TheEvent()
    {
        yield return new WaitForSeconds(eventLength);
        mainCamera.SetActive(true);
        eventCamera.SetActive(false);
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;
        enemy.SetActive(true);
        if (enemy2 != null)
            enemy2.SetActive(true);
        AudioManger.instance.ButtlerArrivalClip();
        Destroy(gameObject);
    }
}
