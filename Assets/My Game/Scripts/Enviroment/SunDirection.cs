using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class SunDirection : MonoBehaviour
{
    public GameObject Sun;
    public GameObject Moon;
    public Light directionLight;
    public float sunSpeed = 1f;
    private Vector3 originalMoonPos;
    private bool isSunny;

    [Header("Skybox")]
    public Material daySkyBox;
    public Material nightSkyBox;
    public float dawnTransitionSpeed = 1f;
    public float duskTransitionSpeed = 1f;
    public float nightTransitionSpeed = 1f;
    public Color dayColor = new Color(0.557f, 0.557f, 0.557f);
    public Color dawnColor = new Color(0.941f, 0.263f, 0.263f);
    public Color duskColor = new Color(0.557f, 0.557f, 0.557f);
    public Color nightColor = Color.black;
    public float skyRotationSpeed = 10f;

    private float currentXRotation;

    void Start()
    {
        directionLight.intensity = 1;
        Sun.transform.position = new Vector3(0, 2000, 0);
        UnityEngine.RenderSettings.skybox = daySkyBox;
        daySkyBox.SetColor("_Tint", dayColor);
        currentXRotation = 90;
        daySkyBox.SetFloat("_Rotation", 0);
        nightSkyBox.SetFloat("_Rotation", 0);
    }
    void Update()
    {
        currentXRotation += sunSpeed * Time.deltaTime;
        if (currentXRotation >= 360)
        {
            currentXRotation -= 360f;
        }

        directionLight.transform.rotation = Quaternion.Euler(currentXRotation, 0, 0);

        if (currentXRotation > 180 && currentXRotation < 360)
        {

            directionLight.intensity = 0;
            isSunny = false;
            UnityEngine.RenderSettings.skybox = nightSkyBox;
            if (currentXRotation > 180 && currentXRotation < 200)
            {
                float t = (currentXRotation - 180) / 20f;
                Color currentNightColor = Color.Lerp(nightColor, dayColor, t * nightTransitionSpeed);
                nightSkyBox.SetColor("_Tint", currentNightColor);
            }
            else if (currentXRotation > 330 && currentXRotation <= 360)
            {
                float t = (currentXRotation - 330) / 30;
                Color currentNightColor = Color.Lerp(dayColor, nightColor, t * nightTransitionSpeed);
                nightSkyBox.SetColor("_Tint", currentNightColor);
            }
            RotationSky();
        }
        else
        {
            RotationSky();
            directionLight.intensity = 1;
            isSunny = true;
            UnityEngine.RenderSettings.skybox = daySkyBox;
            if (currentXRotation > 0 && currentXRotation <= 90)
            {
                float t = ((currentXRotation - 0) / 90);
                Color currentDusktColor = Color.Lerp(nightColor, dayColor, t * duskTransitionSpeed);
                daySkyBox.SetColor("_Tint", currentDusktColor);
            }
            if (currentXRotation > 90 && currentXRotation < 150)
            {
                float t = ((currentXRotation - 90) / 60);
                Color currentDawntColor = Color.Lerp(dayColor, dawnColor, t * dawnTransitionSpeed);
                daySkyBox.SetColor("_Tint", currentDawntColor);
            }
            else if (currentXRotation >= 150 && currentXRotation <= 180)
            {
                float t = ((currentXRotation - 150) / 30);
                Color currentNightColor = Color.Lerp(dawnColor, nightColor, t * dawnTransitionSpeed);
                daySkyBox.SetColor("_Tint", currentNightColor);
            }
        }
    }

    public bool IsSunny()
    {
        return isSunny;
    }
    public void RotationSky()
    {
        float rotationamout = skyRotationSpeed * Time.deltaTime;
        if (isSunny)
        {
            daySkyBox.SetFloat("_Rotation", daySkyBox.GetFloat("_Rotation") + rotationamout);
        }
        else
        {
            nightSkyBox.SetFloat("_Rotation", nightSkyBox.GetFloat("_Rotation") + rotationamout);
        }
    }
}

