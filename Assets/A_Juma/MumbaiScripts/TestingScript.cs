using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    Vector3 pos;
    public GameObject cube;

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

        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            pos = transform.parent.TransformPoint(new Vector3(10, 0, 0));
            Instantiate(cube, pos, cube.transform.rotation);
        }
    

        if (Input.GetKeyDown(KeyCode.X))
        {
            pos = transform.parent.TransformPoint(new Vector3(0, 10, 0));
            Instantiate(cube, pos, cube.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            pos = transform.parent.TransformPoint(new Vector3(0, 0, 10));
            Instantiate(cube, pos, cube.transform.rotation);
        }
    
    
    }
}
