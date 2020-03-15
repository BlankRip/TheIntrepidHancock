using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public static AudioManger instance;
    [Tooltip("The Audio Source through with the player audio will play")]
    [SerializeField] AudioSource playerSoundSource;
    [Tooltip("The game's back tracks audio source")]
    [SerializeField] AudioSource backGroundMusicSource;

    [Tooltip("Audio clip for normal back track")]
    [SerializeField] AudioClip backTrackNormalClip;
    [Tooltip("Audio clip for normal back track")]
    [SerializeField] AudioClip backTrackChaseClip;
    [Tooltip("Audio clips for tutorial insert in order to be played")]
    [SerializeField] AudioClip[] tutorialClips;
    [Tooltip("Audio clips for the player breaking dialogue")]
    [SerializeField] AudioClip[] playerBreakDialogueClips;
    [Tooltip("Audio clips of the playeer grunts")]
    [SerializeField] AudioClip[] playerGrunts;
    [Tooltip("Audio clip of the player when butlers appears")]
    [SerializeField] AudioClip butlerArrivalClip;
    [Tooltip("Audio clip of the player when Mr.Curry appears")]
    [SerializeField] AudioClip curryArrivalClip;

    //-------------------------------------------------------- Think this shoulb be in abby -------------------------------------------
    [Tooltip("The Audio Source through which Mr.Abby will play his dialogue")]
    [SerializeField] AudioSource abbySoundSource;
    [Tooltip("Audio clip abby will play on arrival")]
    [SerializeField] AudioClip abbyArrivalClip;
    public void AbbyArrival()
    {
        PlayOneShotOn(abbySoundSource, abbyArrivalClip);
    }
    //-------------------------------------------------------- Think this shoulb be in abby -------------------------------------------

    //For break dialogues & grunts
    int pick;
    int previousBreakDialogue;
    int previousGrunt;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void PlayTutorialClip(int clipID)
    {
        PlayOneShotOn(playerSoundSource, tutorialClips[clipID]);
    }

    public void PlayBreakDialogues()
    {
        PlayerRandomPlay(playerBreakDialogueClips, previousBreakDialogue);
    }

    public void PlayGrunt()
    {
        PlayerRandomPlay(playerGrunts, previousGrunt);
    }

    public void ButtlerArrival()
    {
        PlayOneShotOn(playerSoundSource, butlerArrivalClip);
    }

    public void CurryArrival()
    {
        PlayOneShotOn(playerSoundSource, curryArrivalClip);
    }





    void PlayOneShotOn(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    void PlayerRandomPlay(AudioClip[] clips, int prieviousPick)
    {
        pick = Random.Range(0, clips.Length - 1);

        if (pick == prieviousPick)
        {
            if (pick == clips.Length - 1)
                pick--;
            else
                pick++;
        }
        prieviousPick = pick;
        playerSoundSource.PlayOneShot(clips[pick]);
    }

}
