using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerBox : MonoBehaviour
{
    [Tooltip("If this trigger box is for tutorial audio")]
    [SerializeField] bool tutorialTriggerBox = true;
    [Tooltip("if this trigger box will have to repeat the clip when entered again")]
    [SerializeField] bool repetable = false;

    [Header("Things needed if tutorial trigger box")]
    [Tooltip("ID of the clip in the array on the sound manager")]
    [SerializeField] int audioClipID = 0;
    [Tooltip("If the clip should play when button is pressed when in trigger collider")]
    [SerializeField] bool activeWithButtonPress;
    [Tooltip("The key needed to be pressed so that the clip will play")]
    [SerializeField] KeyCode activationKey = KeyCode.E;

    bool checkForKey = false;


    private void Update()
    {
        if (Input.GetKeyDown(activationKey) && checkForKey)
        {
            AudioManger.instance.PlayTutorialClip(audioClipID);
            if (!repetable)
                Destroy(gameObject);
            checkForKey = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (tutorialTriggerBox)
            {
                if (!activeWithButtonPress)
                {
                    AudioManger.instance.PlayTutorialClip(audioClipID);
                    if (!repetable)
                        Destroy(gameObject);
                    if (audioClipID == AudioManger.instance.numberOfTutorialClips)
                        AudioManger.instance.playBreakAudio = true;
                }
                else
                    checkForKey = true;
            }
        }
    }
}
