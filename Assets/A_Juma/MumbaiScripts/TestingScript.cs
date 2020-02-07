using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        transform.localPosition = new Vector3(0, 5, 0);

        if (Input.GetKeyDown(KeyCode.W))
        transform.localPosition = new Vector3(10, 0, 0);

        if (Input.GetKeyDown(KeyCode.E))
        transform.localPosition = new Vector3(-10, 0, 0);

        if (Input.GetKeyDown(KeyCode.R))
        transform.localPosition = new Vector3(0, 0, 10);
    }
}
