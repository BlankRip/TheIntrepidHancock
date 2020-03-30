using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAt : MonoBehaviour
{
    [SerializeField] GameObject target;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = transform.position - target.transform.position;
        transform.LookAt(transform.position + targetPos);
    }
}
