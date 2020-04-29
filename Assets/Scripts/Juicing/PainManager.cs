using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PainManager : MonoBehaviour
{

    public static PainManager instance;
    public float painFadeSpeed;

    bool inPain;
    ScreenPain screenPain;
    float painValue;

    void Start()
    {
        instance = this;
        GetComponent<PostProcessVolume>().profile.TryGetSettings<ScreenPain>(out screenPain);
    }

    void Update()
    {
        if (inPain) {
            screenPain.painValue.value = painValue;
            painValue -= Time.deltaTime * painFadeSpeed;
            if (painValue < 0){
                inPain = false;
                painValue = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AddPain(0.3f);
        }

    }

    public void AddPain(float value) {
        inPain = true;
        painValue += value;
    }

}
