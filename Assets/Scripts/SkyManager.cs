using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngineInternal;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class SkyManager : MonoBehaviour
{
    [Header("Sun")]
    [SerializeField]
    private int sunRiseMaxCount;
    [SerializeField]
    private float sunXRotationMin;
    [SerializeField]
    private float sunXRotationMax;
    [SerializeField]
    private float sunIntensityMin;
    [SerializeField]
    private float sunIntensityMax;
    [Header("Sky")]
    [SerializeField]
    private float skySunSizeMin;
    [SerializeField]
    private float skySunSizeMax;
    [SerializeField]
    private float skyExposureMin;
    [SerializeField]
    private float skyExposureMax;
    [SerializeField]
    private float skyAtmoThickMin;
    [SerializeField]
    private float skyAtmoThickMax;
    [Header("LightUp")]
    [SerializeField]
    private float lightUpTime;
    private Light sun;
    private Material sky;
    private int currentSunRiseCount;
    public System.Action OnGameClear;
    private Coroutine sunRiseXRotationCoroutine;
    private Coroutine sunRiseIntensityCoroutine;
    private Coroutine sunRiseSunSizeCoroutine;
    private Coroutine sunRiseExposureCoroutine;
    private Coroutine sunRiseAtmoThickCoroutine;

    private float currentXRotation;
    private float currentIntensity;
    private float currentSunSize;
    private float currentExposure;
    private float currentAtmoThick;

    [ContextMenu("불키기")]
    public void CityLightUp()
    {
        currentSunRiseCount++;
        float currentSunRiseRatio = (float)currentSunRiseCount / sunRiseMaxCount;
        Debug.Log("city light up " + currentSunRiseRatio);

        if (sunRiseXRotationCoroutine != null) StopCoroutine(sunRiseXRotationCoroutine);
        sunRiseXRotationCoroutine = StartCoroutine(SunRiseXRotation(currentSunRiseRatio));

        if (sunRiseIntensityCoroutine != null) StopCoroutine(sunRiseIntensityCoroutine);
        sunRiseIntensityCoroutine = StartCoroutine(SunRiseIntensity(currentSunRiseRatio));

        if (sunRiseSunSizeCoroutine != null) StopCoroutine(sunRiseSunSizeCoroutine);
        sunRiseSunSizeCoroutine = StartCoroutine(SunRiseSunSize(currentSunRiseRatio));

        if (sunRiseExposureCoroutine != null) StopCoroutine(sunRiseExposureCoroutine);
        sunRiseExposureCoroutine = StartCoroutine(SunRiseExposure(currentSunRiseRatio));

        if (sunRiseAtmoThickCoroutine != null) StopCoroutine(sunRiseAtmoThickCoroutine);
        sunRiseAtmoThickCoroutine = StartCoroutine(sunRiseAtmoThick(currentSunRiseRatio));

        if (currentSunRiseCount ==  sunRiseMaxCount)
        {
            OnGameClear.Invoke();
        }
    }
    public void Initialize()
    {
        sun = GetComponent<Light>();
        sky = new Material(RenderSettings.skybox);
        RenderSettings.skybox = sky;
        currentSunRiseCount = 0;

        sky.SetFloat("_SunSize", skySunSizeMin);
        sky.SetFloat("_Exposure", skyExposureMin);
        sky.SetFloat("_AtmosphereThickness", skyAtmoThickMax);
        transform.rotation = Quaternion.Euler(sunXRotationMin, 0, 0);
        sun.intensity = sunIntensityMin;

        currentSunSize = skySunSizeMin;
        currentExposure = skyExposureMin;
        currentXRotation = sunXRotationMin;
        currentIntensity = sunIntensityMin;
        currentAtmoThick = skyAtmoThickMax;
    }

    private IEnumerator SunRiseXRotation(float currentSunRiseRatio)
    {
        float target = (sunXRotationMax -  sunXRotationMin) * currentSunRiseRatio + sunIntensityMin;
        float count = 0f;
        float start = currentXRotation;
        while (count < lightUpTime)
        {
            count += Time.deltaTime;
            currentXRotation = Mathf.Lerp(start, target, count / lightUpTime);
            transform.rotation = Quaternion.Euler(currentXRotation, 0, 0);
            yield return null;
        }
        currentXRotation = target;
        transform.rotation = Quaternion.Euler(currentXRotation, 0, 0);
    }

    private IEnumerator SunRiseIntensity(float currentSunRiseRatio)
    {
        float target = (sunIntensityMax - sunIntensityMin) * currentSunRiseRatio + sunIntensityMin;
        float count = 0f;
        float start = currentIntensity;
        while (count < lightUpTime)
        {
            count += Time.deltaTime;
            currentIntensity = Mathf.Lerp(start, target, count / lightUpTime);
            sun.intensity = currentIntensity; 
            yield return null;
        }
        currentIntensity = target;
        sun.intensity = currentIntensity; 

    }
    private IEnumerator SunRiseSunSize(float currentSunRiseRatio)
    {
        float target = (skySunSizeMax - skySunSizeMin) * currentSunRiseRatio + skySunSizeMin;
        float count = 0f;
        float start = currentSunSize;
        while (count < lightUpTime)
        {
            count += Time.deltaTime;
            currentSunSize = Mathf.Lerp(start, target, count / lightUpTime);
            sky.SetFloat("_SunSize", currentSunSize);
            yield return null;
        }
        currentSunSize = target;
        sky.SetFloat("_SunSize", currentSunSize);
    }
    private IEnumerator SunRiseExposure(float currentSunRiseRatio)
    {
        float target = (skyExposureMax - skyExposureMin) * currentSunRiseRatio + skyExposureMin;
        float count = 0f;
        float start = currentExposure;
        while (count < lightUpTime)
        {
            count += Time.deltaTime;
            currentExposure = Mathf.Lerp(start, target, count / lightUpTime);
            sky.SetFloat("_Exposure", currentExposure);
            yield return null;
        }
        currentExposure = target;
        sky.SetFloat("_Exposure", currentExposure);
    }
    private IEnumerator sunRiseAtmoThick(float currentSunRiseRatio)
    {
        float target = (skyAtmoThickMin - skyAtmoThickMax) * currentSunRiseRatio + skyAtmoThickMax;
        float count = 0f;
        float start = currentAtmoThick;
        while (count < lightUpTime)
        {
            count += Time.deltaTime;
            currentAtmoThick = Mathf.Lerp(start, target, count / lightUpTime);
            sky.SetFloat("_AtmosphereThickness", currentAtmoThick);
            yield return null;
        }
        currentAtmoThick = target;
        sky.SetFloat("_AtmosphereThickness", currentAtmoThick);
    }
}
