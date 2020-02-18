using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPieceExplosion : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float explosionForce = 7;
    [Range(0.0f, 0.5f)] [SerializeField] float explosionRaious = 0.01f;
    [SerializeField] float upWardsModifier = 0.00001f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddExplosionForce(explosionForce, transform.position, explosionRaious, upWardsModifier, ForceMode.Impulse);
    }
}
