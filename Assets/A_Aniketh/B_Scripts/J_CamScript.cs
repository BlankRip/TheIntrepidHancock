using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_CamScript : MonoBehaviour
{
    [Header("Things needed to move camera around")]
    [SerializeField] Transform target;                       //The that will be followed and rotated
    [SerializeField] float mouseSensitivity = 2;             //The mouse sensitivity, AKA rotation speed multiplier
    [SerializeField] float verticalClampMin = -15.0f;        //Minimum vertical movement possible
    [SerializeField] float verticalClampMax = 60.0f;         //Maximum vertical movement possible
    [SerializeField] Vector3 offSet;                         //Offset where the cam will look at
    Vector3 lookAtPosition;
    float mouseX;                                            //The current horizontal input value for horizontal rotaion
    float mouseY;                                            //The current vertical input value for the vertical rotation
    
    [Header("Things needed to do wall clipping for the camera")]
    [SerializeField] float minDistance = 1.0f;               //Minimum distance the object will be from the target
    [SerializeField] float maxDistance = 8.0f;               //Maximum distance the object can be from the target
    [SerializeField] float smoothCamMovement = 10.0f;        //Smoothening done while lerping the object to desired position
    [SerializeField] LayerMask WallClipLayerMask;            //The layers that work for wall clipping
    [SerializeField] Vector3 dollyDirAdjustment;             //To adjust direction of the camera
    float distance;                                          //The current distance the object will be from the target
    Vector3 dollyDir;                 //vector3 that stores the local unit direction the object is from the camera
    Vector3 desiredCameraPos;         //The expected camera postion, used in linear cast as end point of the ray check
    RaycastHit hit;                   //Object that stores details of the objects it hit during the linear cast

    void Awake()
    {
        
        Cursor.visible = false;                                     //Setting cursor to not be visible when playing the game
        Cursor.lockState = CursorLockMode.Locked;                   //Locking the cursor to the center of the screen so that it does not move out of the window
        //For wall clipping
        dollyDir = transform.localPosition.normalized;              //Getting the local unit direction vector
        distance = transform.localPosition.magnitude;               //Getting the local magnitude to the postion with is basically distance from parent to camera
    }

    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;               //Getting horizontal movement input of the mouse
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;               //Getting vertical movement input of the mouse
        mouseY = Mathf.Clamp(mouseY, verticalClampMin, verticalClampMax);    //Clamping the vertical value
        lookAtPosition = new Vector3(target.position.x + offSet.x, target.position.y + offSet.y, target.position.z + offSet.z);               //Setting location to look at with the offset
        transform.LookAt(lookAtPosition);       //Setting object to always look at target

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);        //Rotating the target based on horizontal and vertical mouse input values

        // Clamping camera 
        desiredCameraPos = target.transform.TransformPoint(dollyDir * maxDistance);       //Local position fo the vector; Getting vector by multyping the direcion and magnitude

        // Check if there is a wall or object between the camera and move the camera close to the target if so else set the camera to be at the normal distance from the target
        if (Physics.Linecast(target.transform.position, desiredCameraPos, out hit, WallClipLayerMask))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, (dollyDir + dollyDirAdjustment) * distance, Time.deltaTime * smoothCamMovement);   //Lerping postion to the desired spot
    }
}
