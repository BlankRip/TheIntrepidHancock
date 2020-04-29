using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySideHitEffect : MonoBehaviour
{
    public ParticleSystem slapEffect;


    public void OnHitAction()
    {
        slapEffect.Play();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnHitAction();
        }
    }

}
