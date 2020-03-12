using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrail : MonoBehaviour
{
    [Tooltip("The amount of time before each fade is spawned")] 
    [SerializeField] float timeBetweenFades;
    [Tooltip("The max number of fades that can spawn in 1 trail")] 
    [SerializeField] int maxNumberOfFades;
    [Tooltip("The time before the spawned fade is destroyed")] 
    [SerializeField] float distroyFadeAfter = 1.5f;
    [Tooltip("The Prefab That needs to be spawned")] 
    [SerializeField] GameObject fadePrefab;
    [Tooltip("Bool that check if the fade trails should run ")] 
    [SerializeField] bool startFadeEffectOnAwake;

    int currentFadeNumber;                               //Tracks the number of fades spawned in a trail
    float currentTime;                                   //Tracks the time sice the last fade (this is timer)
    GameObject instanciatedObject;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeBetweenFades;
        currentFadeNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startFadeEffectOnAwake)
        {
            if (currentTime < 0 && currentFadeNumber < maxNumberOfFades)
            {
                instanciatedObject = Instantiate(fadePrefab, transform.position, transform.rotation);
                instanciatedObject.transform.localScale = transform.localScale;
                Destroy(instanciatedObject, distroyFadeAfter);
                currentTime = timeBetweenFades;       //Resetting timer
                currentFadeNumber++;                  //Setting the number of fade tracker value
            }
            else
                currentTime -= Time.deltaTime;        //Reducing the time
        }
        else
            currentFadeNumber = 0;                    //setting nubmer of fades tracker back to 0 for the next trail
    }
}
