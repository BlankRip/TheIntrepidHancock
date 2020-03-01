using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrail : MonoBehaviour
{
    [SerializeField] float timeBetweenFades;             //The amount of time before each fade is spawned
    float currentTime;                                   //Tracks the time sice the last fade (this is timer)
    [SerializeField] int numberOfFades;                  //The max number of fades that can spawn in 1 trail
    [SerializeField] float distroyFadeAfter = 1.5f;
    int currentFadeNumber;                               //Tracks the number of fades spawned in a trail
    [SerializeField] GameObject fadePrefab;
    GameObject instanciatedObject;
    [SerializeField] bool startFadeEffectOnAwake;     //Bool that check if the fade trails should run 


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
            if (currentTime < 0 && currentFadeNumber < numberOfFades)
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
