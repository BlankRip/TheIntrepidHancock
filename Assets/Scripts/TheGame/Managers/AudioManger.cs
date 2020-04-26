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

    //-------------------------------------------------------- The cenematic camera will hold this -------------------------------------------
    [Tooltip("Audio clip abby will play on arrival")]
    [SerializeField] AudioClip abbyArrivalClip;
    public void AbbyArrival(AudioSource source)
    {
        PlayClipOn(source, abbyArrivalClip);
    }
    //-------------------------------------------------------- The cenematic camera will hold this -------------------------------------------

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
        PlayClipOn(playerSoundSource, tutorialClips[clipID]);
    }

    public void ButtlerArrivalClip()
    {
        PlayClipOn(playerSoundSource, butlerArrivalClip);
    }

    public void PlayBreakDialoguesClip()
    {
        if(playBreakAudio)
        {
            pick = Random.Range(0, 100);
            Debug.Log("<color=pink>" + pick + "</color>");
            if ((pick < 6) || (pick > 34 && pick < 38) || (pick > 59 && pick < 63) || (pick > 85 && pick < 91))
                PlayerRandomPlay(playerBreakDialogueClips, ref previousBreakDialogue);
        }
    }

    public void SwitchToChase()
    {
        StopCoroutine("SwithTracks");
        StartCoroutine(SwithTracks(backTrackChaseClip, 0.009f, 0.2f));
    }

    public void SwitchToCalm()
    {
        StopCoroutine("SwithTracks");
        StartCoroutine(SwithTracks(backTrackNormalClip, 0.005f, 0.27f));
    }

    public void PlayGruntClip()
    {
        PlayerRandomPlay(playerGrunts, ref previousGrunt);
    }

    public void RelicCollectedClip()
    {
        PlayClipOn(playerSoundSource, relicCollectedClips[relicClip]);
        relicClip++;
    }

    public void CurryArrivalClip()
    {
        PlayClipOn(playerSoundSource, curryArrivalClip);
    }

    public void ExitDungeonClip()
    {
        PlayClipOn(playerSoundSource, ExitClip);
    }




    void PlayClipOn(AudioSource source, AudioClip clip)
    {
        if (instance != null)
        {
            source.Stop();
            source.clip = clip;
            source.Play();
        }
    }

    void PlayerRandomPlay(AudioClip[] clips, ref int prieviousPick)
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
            playerSoundSource.Stop();
            playerSoundSource.clip = clips[pick];
            playerSoundSource.Play();
        }
    }

    IEnumerator SwithTracks(AudioClip ClipToSwitch, float increaseVolumeBy, float maxVolume)
    {
        currentVolume = backGroundMusicSource.volume;
        while (currentVolume > 0)
        {
            backGroundMusicSource.volume = currentVolume;
            yield return new WaitForSeconds(0);
            currentVolume -= increaseVolumeBy;
        }

        backGroundMusicSource.clip = ClipToSwitch;
        backGroundMusicSource.Play();

        while(currentVolume < maxVolume)
        {
            backGroundMusicSource.volume = currentVolume;
            yield return new WaitForSeconds(0);
            currentVolume += increaseVolumeBy;
        }
    }

}
