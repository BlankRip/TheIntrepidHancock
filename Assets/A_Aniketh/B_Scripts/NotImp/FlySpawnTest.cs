using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySpawnTest : MonoBehaviour
{
    [SerializeField] GameObject fireFly;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Instantiate(fireFly, transform.position, Quaternion.identity);
    }
}
