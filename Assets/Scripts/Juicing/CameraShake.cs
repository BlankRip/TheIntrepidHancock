using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake instance;

    public float maxDisplacement, shakeDecay, shakeFrequency;

    Transform shakeParent;
    float shakeFactor;
    Vector3 shakeVector;
    bool isShaking;
    bool impactfulShake;

    void Start()
    {
        instance = this;
        shakeParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(isShaking){
            shakeVector = (shakeParent.up * Mathf.Sin(Time.time * shakeFrequency) + shakeParent.right * Mathf.Sin((Time.time * shakeFrequency) + 51)).normalized * maxDisplacement * shakeFactor;
            transform.position = shakeParent.position + shakeVector;
            shakeFactor -= Time.deltaTime * shakeDecay;
            if(shakeFactor < 0)
            {
                isShaking = false;
                if (impactfulShake)
                    impactfulShake = false;
                transform.position = shakeParent.position;
            }
        }

    }

    public void ShakeCamera(float value)
    {
        isShaking = true;
        shakeFactor += value;
    }

    public void ShakeCamera(float value, float limit, bool impactful)
    {
        isShaking = true;
        if (!impactfulShake)
        {
            shakeFactor += value;
            if (shakeFactor > limit) shakeFactor = limit;
        }
        
        if (impactful)
            impactfulShake = true;
    }

}
