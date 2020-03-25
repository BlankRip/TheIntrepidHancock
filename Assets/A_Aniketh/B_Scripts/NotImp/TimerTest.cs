using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    public float timer = 50;
    [SerializeField] float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            Debug.Log("<color=red> Done </color>");
            Destroy(this);
        }
        timer -= Time.deltaTime * speed;
    }
}
