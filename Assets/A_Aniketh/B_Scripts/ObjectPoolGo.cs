using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolGo : MonoBehaviour
{
    Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    int previousPick;
    [SerializeField] float speed = 5;
    [SerializeField] ParticleSystem[] effectObjectPoolList;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            int pickAnEffect = Random.Range(0, effectObjectPoolList.Length);
            if(pickAnEffect == previousPick)
            {
                if (pickAnEffect == effectObjectPoolList.Length - 1)
                    pickAnEffect--;
                else
                    pickAnEffect++;
            }
            previousPick = pickAnEffect;
            effectObjectPoolList[pickAnEffect].transform.position = transform.position + Vector3.right * 2;
            effectObjectPoolList[pickAnEffect].Stop();
            effectObjectPoolList[pickAnEffect].Play();
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(horizontalInput, 0, verticalInput) * speed;
    }
}
