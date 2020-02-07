using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTesting : MonoBehaviour
{
    public Transform target, player;
    public float mouseX;
    public float mouseY;
    float rotSpeed = 2;

    public float verticalClampMin;
    public float verticalClampMax;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //wall collision
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * rotSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
        mouseY = Mathf.Clamp(mouseY, verticalClampMin, verticalClampMax);

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        // 
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}

/*
{
    // camera smooth//sphere cast
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

    // camera rotation =========================================// ALL VALUES ARE PUBLIC BECAUSE OF TESTING REASONS IN THE EDITOR.
    /*
    public Transform target, player;                            // 
    public float mouseX;                                        //
    public float mouseY;                                        //
    float rotSpeed = 2;                                         //

    public float zoom;                                          // Base Zoom value
    public float zoomSpeed;                                     

    public float zoomMin;                                       // Minimum Zoom
    public float zoomMax;                                       // Maximum Zoom

    public float verticalClampMin;                              // Y AXIS Clamp MIN
    public float verticalClampMax;                              // Y AXIS Clamp MAX

    //=========
    public float smoothTime = 0.3F;                             //
    private Vector3 velocity = Vector3.zero;                    //

    //==== wall collision
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //wall collision
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

   // void Update()
    //{
        /*
        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; //zoom will increase or decrease depending on where the scroll is + / -
        // === CLAMPING ZOOMS ===
        if (zoom > zoomMin)
        {
            zoom = zoomMin;
        }

        if (zoom < zoomMax)
        {
            zoom = zoomMax;
        }
        // === CLAMPING ZOOMS ===
        transform.localPosition = new Vector3(0, 0, zoom); // explain 
        */
        /*
        mouseX += Input.GetAxis("Mouse X") * rotSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
        mouseY = Mathf.Clamp(mouseY, verticalClampMin, verticalClampMax); //Control mouseY Clamp between 2 values MIN & MAX

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0); //storing quaternion rotations with mousey and mousex
    */
                                                               //player.rotation = Quaternion.Euler(0, mouseX, 0); // enable this to let the player rotate with cam
                                                               //transform.position = Vector3.Lerp(transform.position, transform.position, Time.deltaTime);
                                                               //====================================
                                                               // camera smooth
                                                               // Define a target position above and behind the target transform
                                                               //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

        // Smoothly move the camera towards that target position
        //transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);


        //========================wall collision
        /*
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}*/
