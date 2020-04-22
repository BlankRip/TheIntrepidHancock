using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioManger.instance.ExitDungeonClip();
        SceneShifter.LoadScene("VictoryScene");
    }
}
