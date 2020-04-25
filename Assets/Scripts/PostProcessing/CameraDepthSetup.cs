using UnityEngine;

public class CameraDepthSetup : MonoBehaviour
{

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

}
