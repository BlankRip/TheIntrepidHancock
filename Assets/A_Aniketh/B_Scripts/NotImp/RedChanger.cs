using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedChanger : MonoBehaviour
{
    [SerializeField] Material switchMat;
    [SerializeField] AudioSource landed;
    [SerializeField] AudioSource left;
    [SerializeField] bool playingThis;
    Renderer renderer;
    RedTracker tracker;
    bool done;

    void Start()
    {
        if(playingThis)
        {
            done = false;
            renderer = GetComponent<Renderer>();
            tracker = FindObjectOfType<RedTracker>();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!done && playingThis)
        {
            landed.Play();
            renderer.material = switchMat;
            tracker.numberSwitched++;
            done = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (playingThis)
            left.Play();
    }
}
