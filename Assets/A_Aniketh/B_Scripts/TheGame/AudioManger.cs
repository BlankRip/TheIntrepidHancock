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
    [Tooltip("Audio clips played when each relic is collected")]
    [SerializeField] AudioClip[] relicCollectedClips;
    [Tooltip("Audio clip of the player when butlers appears")]
    [SerializeField] AudioClip butlerArrivalClip;
    [Tooltip("Audio clip of the player when Mr.Curry appears")]
    [SerializeField] AudioClip curryArrivalClip;
    [Tooltip("Exit Dungeon clip the player will play when leaving")]
    [SerializeField] AudioClip ExitClip;

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

    //For changeing game background clips
    float currentVolume;

    //For break dialogues & grunts
    int pick;
    int previousBreakDialogue;
    int previousGrunt;
    [HideInInspector] public int numberOfTutorialClips;
    [HideInInspector] public bool playBreakAudio = false;

    //For relic collection
    int relicClip = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        numberOfTutorialClips = tutorialClips.Length - 1;
    }

    //---------------------------------------------------------- FOR TESTING ------------------------------------------------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SwitchToChase();

        if (Input.GetKeyDown(KeyCode.Y))
            SwitchToCalm();
    }

    //---------------------------------------------------------- FOR TESTING ------------------------------------------------

    public void PlayTutorialClip(int clipID)
    {
        PlayOneShotOn(playerSoundSource, tutorialClips[clipID]);
    }

    public void ButtlerArrivalClip()
    {
        PlayOneShotOn(playerSoundSource, butlerArrivalClip);
    }

    public void PlayBreakDialoguesClip()
    {
        if(playBreakAudio)
        {
            pick = Random.Range(0, 100);
            Debug.Log("<color=pink>" + pick + "</color>");
            if ((pick < 10) || (pick > 35 && pick < 40) || (pick > 59 && pick < 65) || (pick > 87 && pick < 92))
                PlayerRandomPlay(playerBreakDialogueClips, previousBreakDialogue);
        }
    }

    public void SwitchToChase()
    {
        StopCoroutine("SwithTracks");
        StartCoroutine(SwithTracks(backTrackChaseClip));
    }

    public void SwitchToCalm()
    {
        StopCoroutine("SwithTracks");
        StartCoroutine(SwithTracks(backTrackNormalClip));
    }

    public void PlayGruntClip()
    {
        PlayerRandomPlay(playerGrunts, previousGrunt);
    }

    public void RelicCollectedClip()
    {
        PlayOneShotOn(playerSoundSource, relicCollectedClips[relicClip]);
        relicClip++;
    }

    public void CurryArrivalClip()
    {
        PlayOneShotOn(playerSoundSource, curryArrivalClip);
    }

    public void ExitDungeonClip()
    {
        PlayOneShotOn(playerSoundSource, ExitClip);
    }




    void PlayOneShotOn(AudioSource source, AudioClip clip)
    {
        if (instance != null)
            source.PlayOneShot(clip);
    }

    void PlayerRandomPlay(AudioClip[] clips, int prieviousPick)
    {
        if (instance != null)
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

    IEnumerator SwithTracks(AudioClip ClipToSwitch)
    {
        currentVolume = backGroundMusicSource.volume;
        while (currentVolume > 0)
        {
            Debug.Log("<color=red>IN 1</color>");
            backGroundMusicSource.volume = currentVolume;
            yield return new WaitForSeconds(0);
            currentVolume -= 0.002f;
        }

        Debug.Log("<color=red>Out 1</color>");
        backGroundMusicSource.clip = ClipToSwitch;
        backGroundMusicSource.Play();

        while(currentVolume < 0.2f)
        {
            Debug.Log("<color=red>IN 2</color>");
            backGroundMusicSource.volume = currentVolume;
            yield return new WaitForSeconds(0);
            currentVolume += 0.002f;
        }
        Debug.Log("<color=red>Ended Switch</color>");
    }

}
