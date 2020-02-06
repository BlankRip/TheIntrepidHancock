using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTesting : MonoBehaviour
{
    // camera smooth
    /*
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset;

    void Update()
    {
        // camera smooth
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(offset); //target.TransformPoint(new Vector3(0, 5, -10));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetPosition, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}*/

    //==============================================================================================================================================================================================================================
    // camera zoom & rotations
    /*
    public Transform playerCam;
    public Transform targetCenter;
    public Transform playerPos;
    public float zoom = 0;
    public float zoomSpeed = 2;

    public float zoomMin = -2f;
    public float zoomMax = -10f;

    float mouseX;
    float mouseY;
    float mouseSpeed = 5f;

    void Update()
    {
        // camera zoom & rotations 
        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (zoom > zoomMin)
        {
            zoom = zoomMin;
        }

        if (zoom < zoomMax)
        {
            zoom = zoomMax;
        }

        playerCam.transform.localPosition = new Vector3 (targetCenter.position.x, targetCenter.position.y, targetCenter.position.z + zoom);
        if (Input.GetMouseButton (1))
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed; // -= Inverse
        }

        targetCenter.localRotation = Quaternion.Euler(mouseY, mouseX, 0);

        playerCam.LookAt(targetCenter);
    }*/

    // camera rotation ============================================
    public Transform target, player;
    public float mouseX;
    public float mouseY;
    float rotSpeed = 2;

    public float zoom;
    public float zoomSpeed;

    public float zoomMin;
    public float zoomMax;

    public float verticalClampMin;
    public float verticalClampMax;

    //=========
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (zoom > zoomMin)
        {
            zoom = zoomMin;
        }

        if (zoom < zoomMax)
        {
            zoom = zoomMax;
        }

        transform.localPosition = new Vector3(0, 0, zoom); // explain

        mouseX += Input.GetAxis("Mouse X") * rotSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
        mouseY = Mathf.Clamp(mouseY, verticalClampMin, verticalClampMax);

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        //player.rotation = Quaternion.Euler(0, mouseX, 0); // enable this to let the player rotate with cam
        //transform.position = Vector3.Lerp(transform.position, transform.position, Time.deltaTime);
        //====================================
        // camera smooth
        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

        // Smoothly move the camera towards that target position
        //transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);

    }
}

