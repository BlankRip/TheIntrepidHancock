using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAt : MonoBehaviour
{
    EquipManager equiper;
    [SerializeField] GameObject target;
    [SerializeField] float bounceHight;
    public float bounceTracker = 0;
    float initialY;
    bool up = true;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        equiper = FindObjectOfType<EquipManager>();
        target = Camera.main.gameObject;
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(equiper.resetUI)
        {
            initialY = transform.position.y;
            equiper.resetUI = false;
        }

        if (up)
            bounceTracker += Time.deltaTime;
        else
            bounceTracker -= Time.deltaTime;

        if (up && bounceTracker > bounceHight)
            up = false;
        else if (!up && bounceTracker < -bounceHight)
            up = true;

        transform.position = new Vector3(transform.position.x, initialY + bounceTracker, transform.position.z);
        targetPos = transform.position - target.transform.position;
        transform.LookAt(transform.position + targetPos);
    }
}
