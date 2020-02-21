using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    ScoreScript score_relic;                                                //Script that keeps track of the score and status of relic spawns
    [SerializeField] GameObject fractureVersion;                            //Fractured version if the prop
    [SerializeField] GameObject relic;                                      //The relic gameObject
    [SerializeField] GameObject fireFlies;                                  //The fire fly prefab
    [SerializeField] int scoreToAdd;                                        //The amount of score given when this object is broken
    [SerializeField] int hitsBeforeBreak = 2;                               //Number of its it takes before breaking
    [Range(1, 2)] [SerializeField] float increaseSizeBy = 1.1f;             //For hit effect how much of the size is increased on-hit
    [Range(0, 0.5f)] [SerializeField] float maxMovePosition = 0.1f;         //For hit effect the max the object can move from its initial postion 
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
        movePosition = Random.Range(0, maxMovePosition);
        addPosition = new Vector3(movePosition, 0, -movePosition);
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
                transform.position = initialPosition + addPosition;
                StartCoroutine(BackToOrigial());
            }

            if (hitsTaken > hitsBeforeBreak)
            {
                score_relic.currentScore += scoreToAdd;
                Instantiate(fractureVersion, transform.position, transform.rotation);
                Instantiate(fireFlies, transform.position, Quaternion.identity);                    //Spawn fire fly
                //If player cross the score thresh hold to spawn the relic then spawn the relic
                if (score_relic.spawnRelic)
                {
                    Instantiate(relic, transform.position, transform.rotation);
                    score_relic.spawnRelic = false;
                }
                Instantiate(fireFlies, transform.position, Quaternion.identity);                    //Spawn fire fly
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
