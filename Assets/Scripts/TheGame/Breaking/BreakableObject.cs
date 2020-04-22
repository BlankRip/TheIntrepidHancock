using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("Fractured version if the prop")] 
    [SerializeField] GameObject fractureVersion;
    [Tooltip("The relic gameObject")] 
    [SerializeField] GameObject relic;
    [Tooltip("The breaking audio clips")]
    [SerializeField] AudioClip[] breakingClips;
    [Tooltip("The amount of score given when this object is broken")] 
    [SerializeField] int scoreToAdd;
    [Tooltip("Number of its it takes before breaking")] 
    [SerializeField] int hitsBeforeBreak = 2;
    [Tooltip("For hit effect how much of the size is increased on-hit")] [Range(1, 2)] 
    [SerializeField] float increaseSizeBy = 1.1f;
    [Tooltip("For hit effect the max the object can move from its initial postion")]
    [Range(0, 0.5f)] [SerializeField] float maxMovePosition = 0.1f;

    AudioSource breakingSource;
    ParticleSystem breakVFX;
    Vector3 breakEffectPos;
    ScoreScript score_relic;                 //Script that keeps track of the score and status of relic spawns
    Vector3 initialSize;                    //The initial size to which it should get back to during the hit effect
    Vector3 initialPosition;                //The initial postion to which the object should get back during the hit effect
    Vector3 addPosition;                   //The vector added to initial position to move the obejct
    float movePosition;                    //The values on the addPosition vectore picked at random
    int hitsTaken;                         //Number of hits taken so far

    void Start()
    {
        hitsTaken = 0;
        initialSize = transform.localScale;
        initialPosition = transform.position;
        if (gameObject.GetComponent<MeshRenderer>() != null)
            breakEffectPos = gameObject.GetComponent<MeshRenderer>().bounds.center;
        else
            breakEffectPos = transform.position;
        score_relic = FindObjectOfType<ScoreScript>();

    }

    private void OnTriggerEnter(Collider other)
    {
        //If colliding with weapon then display hit effect or break it if final blow
        if (other.tag == "Weapon")
        {
            Debug.Log("<color=red>Hitting</color>");
            if (hitsTaken <= hitsBeforeBreak)
            {
                hitsTaken++;
                transform.localScale = initialSize * increaseSizeBy;
                movePosition = Random.Range(0, maxMovePosition);
                addPosition = new Vector3(movePosition, 0, -movePosition);
                transform.position = initialPosition + addPosition;
                StartCoroutine(BackToOrigial());
            }

            if (hitsTaken > hitsBeforeBreak)
            {
                //Playing the breaking sound effect
                //breakingSource.transform.SetParent(null);
                breakingSource = ObjectPool.instance.SpawnPoolObj("BreakAudioSource", transform.position, Quaternion.identity).GetComponent<AudioSource>();
                for (int i = 0; i < breakingClips.Length; i++)
                {
                    breakingSource.PlayOneShot(breakingClips[i]);
                }
                //Destroy(breakingSource.gameObject, 7.0f);

                score_relic.currentScore += scoreToAdd;
                Instantiate(fractureVersion, transform.position, transform.rotation);

                breakVFX = ObjectPool.instance.SpawnPoolObj("PotBreakParticles", breakEffectPos, Quaternion.identity).GetComponent<ParticleSystem>();
                breakVFX.Stop();
                breakVFX.Play();

                ObjectPool.instance.SpawnPoolObj("FireFlies", transform.position, Quaternion.identity);
                //If player cross the score thresh hold to spawn the relic then spawn the relic
                if (score_relic.spawnRelic)
                {
                    Instantiate(relic, transform.position, transform.rotation);
                    score_relic.spawnRelic = false;
                }
                ObjectPool.instance.SpawnPoolObj("FireFlies", transform.position, Quaternion.identity);
                AudioManger.instance.PlayBreakDialoguesClip();
                Destroy(gameObject);
            }
        }
    }

    //IEnumerator to bring the transforms of the object back to its original state during the hit effect
    IEnumerator BackToOrigial()
    {
        yield return new WaitForSeconds(0.01f);
        transform.localScale = initialSize;
        transform.position = initialPosition;
    }
}
