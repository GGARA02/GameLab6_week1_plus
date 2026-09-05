using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ArrowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float smoothTime;

    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Volume defaultVolume;
    [SerializeField] 
    private Volume dashVolume;
    [SerializeField] 
    private Volume hyperDashVolume;
    [SerializeField] 
    private Volume bulletTimeVolume;
    [SerializeField]
    private float time = 0.1f;
    [SerializeField]
    private float defaultFOV;
    [SerializeField]
    private float dashFOV;
    [SerializeField]
    private float hyperDashFOV;
    [SerializeField]
    private float bulletTimeFOV;

    private Vector3 currentCameraSpeed;
    private float currentFov;

    private Coroutine fovChangeCoroutine;
    private Coroutine volumeChangeCoroutine;
    private Volume volume;
    private ArrowController arrowController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetWorldPos = target.TransformPoint(offset);
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetWorldPos, ref currentCameraSpeed, smoothTime);
        //newPosition.x = Mathf.Round(newPosition.x / 0.1f) * 0.1f;
        //newPosition.y = Mathf.Round(newPosition.y / 0.1f) * 0.1f;
        //newPosition.z = Mathf.Round(newPosition.z / 0.1f) * 0.1f;
        transform.position = newPosition;
        Vector3 lookTarget = target.transform.position;
        //lookTarget.x = Mathf.Round(newPosition.x / 0.1f) * 0.1f;
        //lookTarget.y = Mathf.Round(newPosition.y / 0.1f) * 0.1f;
        //lookTarget.z = Mathf.Round(newPosition.z / 0.1f) * 0.1f;
        Debug.Log(lookTarget);
        transform.LookAt(lookTarget);
        cam.fieldOfView = currentFov;
    }

    public void Initialize(ArrowController controller)
    {
        cam = GetComponent<Camera>();
        arrowController = controller;
        volume = defaultVolume;
        volume.weight = 1f;
        transform.position = target.TransformPoint(offset);
        transform.LookAt(target.transform.position);
        currentCameraSpeed = Vector3.zero;
        controller.OnArrowStateChange += ArrowStateFollowCam;
        currentFov = defaultFOV;
    }

    private void ArrowStateFollowCam(ArrowState arrowState)
    {
        switch (arrowState)
        {
            case ArrowState.None:
                if (fovChangeCoroutine != null) StopCoroutine(fovChangeCoroutine);
                fovChangeCoroutine = StartCoroutine(FovChangeSmooth(currentFov, defaultFOV, 0.3f));

                if (volumeChangeCoroutine != null) StopCoroutine(volumeChangeCoroutine);
                volumeChangeCoroutine = StartCoroutine(VolumeChangeSmooth(arrowState));
                break;
            case ArrowState.Dash:
                if (fovChangeCoroutine != null) StopCoroutine(fovChangeCoroutine);
                fovChangeCoroutine = StartCoroutine(FovChangeSmooth(currentFov, dashFOV, 0.4f));

                if (volumeChangeCoroutine != null) StopCoroutine(volumeChangeCoroutine);
                volumeChangeCoroutine = StartCoroutine(VolumeChangeSmooth(arrowState));
                break;
            case ArrowState.HyperDash:
                if (fovChangeCoroutine != null) StopCoroutine(fovChangeCoroutine);
                fovChangeCoroutine = StartCoroutine(FovChangeSmooth(currentFov, hyperDashFOV, 0.2f));

                if (volumeChangeCoroutine != null) StopCoroutine(volumeChangeCoroutine);
                volumeChangeCoroutine = StartCoroutine(VolumeChangeSmooth(arrowState));
                break;
            case ArrowState.BulletTime:
                if (fovChangeCoroutine != null) StopCoroutine(fovChangeCoroutine);
                fovChangeCoroutine = StartCoroutine(FovChangeSmooth(currentFov, bulletTimeFOV, 0.1f));

                if (volumeChangeCoroutine != null) StopCoroutine(volumeChangeCoroutine);
                volumeChangeCoroutine = StartCoroutine(VolumeChangeSmooth(arrowState));
                break;
            default:

                break;
        }
    }

    IEnumerator FovChangeSmooth(float originFov, float targetFov, float time)
    {
        var count = 0f;
        while (count < time)
        {
            count += Time.deltaTime;
            currentFov = Mathf.Lerp(originFov, targetFov, count / time);

            yield return null;
        }
        currentFov = targetFov;
        fovChangeCoroutine = null;
    }

    IEnumerator VolumeChangeSmooth(ArrowState arrowState)
    {
        float count = 0f;
        //대시를 할때는 즉각적으로한다.
        //부드럽게 전환이 필요한것은 대시가 꺼질때와 불렛타임 온오프일때 정도란다. 
        switch (arrowState)
        {
            case ArrowState.None:
                while (count < time)
                {
                    count += Time.deltaTime;
                    volume.weight = Mathf.Lerp(1, 0, count / time);
                    yield return null;
                }
                count = 0;
                volume = defaultVolume;
                break;
            case ArrowState.Dash:
                volume.weight = 0f;
                volume = dashVolume;
                break;
            case ArrowState.HyperDash:
                volume.weight = 0f;
                volume = hyperDashVolume;
                break;
            case ArrowState.BulletTime:
                volume.weight = 0f;
                volume = bulletTimeVolume;
                break;
            default:
                volume = defaultVolume;
                break;
        }
        while (count < time)
        {
            count += Time.deltaTime;
            volume.weight = Mathf.Lerp(0, 1, count / time);
            yield return null;
        }
    }
}
