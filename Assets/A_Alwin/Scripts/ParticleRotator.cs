using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleRotator : MonoBehaviour
{
    public ParticleSystem weaponTrail;

    private void Start()
    {
       
    }

    void FixedUpdate()
    {
        if (weaponTrail)
        {
            var mainPart = weaponTrail.main;
            mainPart.startRotation3D = true;
            Vector3 rotationEuler = transform.eulerAngles;
            mainPart.startRotationX = Mathf.Deg2Rad * rotationEuler.x;
            mainPart.startRotationY = Mathf.Deg2Rad * rotationEuler.y;
            mainPart.startRotationZ = Mathf.Deg2Rad * rotationEuler.z;
        }
    }
}
