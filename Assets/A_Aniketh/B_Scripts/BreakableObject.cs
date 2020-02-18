using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] GameObject fractureVersion;
    [SerializeField] GameObject relic;
    [SerializeField] int scoreToAdd;
    [SerializeField] int hitsBeforeBreak = 2;
    int hitsTaken;

    void Start()
    {
        hitsTaken = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(hitsTaken <= hitsBeforeBreak)
                hitsTaken++;
                //Debug.Log("<color=pink>Hit</color>" + hitsTaken + "<color=pink>BeforeBreak</color>" + hitsBeforeBreak);
            if (hitsTaken > hitsBeforeBreak)
            {
                Instantiate(fractureVersion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            
        }
    }
}
