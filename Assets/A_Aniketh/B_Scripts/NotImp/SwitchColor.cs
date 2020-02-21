using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchColor : MonoBehaviour
{
    [SerializeField] Material switchMat;
    [SerializeField] AudioSource landed;
    [SerializeField] AudioSource left;
    [SerializeField] bool playingThis;
    Material initialMat;
    Renderer renderer;

    void Start()
    {
        if(playingThis)
        {
            renderer = GetComponent<Renderer>();
            initialMat = renderer.material;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(playingThis)
        {
            landed.Play();
            renderer.material = switchMat;
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if(playingThis)
        {
            left.Play();
            renderer.material = initialMat;
        }
    }
}
