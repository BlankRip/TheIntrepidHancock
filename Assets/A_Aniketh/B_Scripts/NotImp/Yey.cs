using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yey : MonoBehaviour
{
    [SerializeField] GameObject show;

    private void OnCollisionEnter(Collision collision)
    {
        show.SetActive(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        show.SetActive(false);
    }
}
