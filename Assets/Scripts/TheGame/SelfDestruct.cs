using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [Tooltip("Time before destring itself")] 
    [SerializeField] float distructionsCountdown;

    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(distructionsCountdown);
        Destroy(gameObject);
    }
}
