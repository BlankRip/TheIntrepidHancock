using UnityEngine;

public class GoldenTablet : MonoBehaviour
{
    public float turnSpeed, emissionFadeValue, fadeSpeed, relicUIActionSpeed;
    public Vector3 rot;
    public Transform angleOne, angleTwo, tabletTransform;
    public Material glowMaterial, normalMaterial;
    public Color glowColor;
    public AnimationCurve relicUIScaleCurve;
    public RectTransform relicUIImage;
    Color darkColor;

    float relicUIScaleTime;

    MeshRenderer meshRenderer;
    bool glowFade, relicUIAction;

    void Start()
    {
        darkColor = Color.black;
        rot = transform.rotation.eulerAngles;
     //   startAngle = rot.y;
    }

    void Update()
    {
        tabletTransform.rotation = Quaternion.Lerp(angleOne.rotation, angleTwo.rotation, (Mathf.Sin(Time.time * turnSpeed) + 1) * 0.5f);

        if(glowFade)
        {
            emissionFadeValue -= Time.deltaTime * fadeSpeed;
            glowMaterial.SetColor("_EmissionColor", Color.Lerp(darkColor, glowColor, emissionFadeValue));

            if(emissionFadeValue < 0)
            {
                glowFade = false;
                meshRenderer.sharedMaterial = normalMaterial;
            }
        }

        if(relicUIAction)
        {
            relicUIImage.localScale = Vector3.one * relicUIScaleCurve.Evaluate(relicUIScaleTime);
            relicUIScaleTime += Time.deltaTime * relicUIActionSpeed;
            if(relicUIScaleTime > 1)
            {
                relicUIAction = false;
            }
        }

    }

    public void NewPieceAddEffect(GameObject newPiece)
    {
        meshRenderer = newPiece.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = glowMaterial;
        glowMaterial.SetColor("_EmissionColor", glowColor);
        relicUIScaleTime = 0;
        emissionFadeValue = 1;
        glowFade = true;
        relicUIAction = true;
    }

}
