using UnityEngine;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    public Image image;
    public bool fadeIn = true, fade = true;
    public float fadeSpeed, value;


    void Update()
    {
        if (fade)
        {
            image.color = Color.black * value;
            value += Time.deltaTime * fadeSpeed * (fadeIn == true ? 1 : -1);

            if (fadeIn && value > 1)
            {
                fade = false;
            }
            else if (!fadeIn && value < 0)
            {
                fade = false;
            }
        }
    }
}
