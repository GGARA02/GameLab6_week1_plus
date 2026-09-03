using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ArrowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody targetRb;
    [SerializeField] private ArrowController controller;
    [SerializeField] private Volume ChargeVolume;

    [Header("Value Setting")]
    [SerializeField]
    private Vector3 Offset = new Vector3(0, 1, -3);
    [SerializeField]
    private float SmoothTime = 0.1f;
    [SerializeField]
    private float ChargingFov = 40f;
    [SerializeField]
    private float DashFov = 70f;
    [SerializeField]
    private float NormalToChargeTime = 0.1f;
    [SerializeField]
    private float ChargeToDashTime = 0.1f;
    [SerializeField]
    private float DashToNormalTime = 0.4f;

    [Header("화면 이펙트 세팅")]
    [SerializeField]
    private float InChargeTime = 0.5f;
    [SerializeField]
    private float OutChargeTime = 0.1f;

    [SerializeField]
    private float ChargingSmooth = 0.01f;
   


    private Camera cam;
    private Vector3 _currentVelocity;

    private float defaultFov;

    private float currentFov { get { return cam.fieldOfView; }
        set { cam.fieldOfView = value; }}

    private float currentSmooth;

    private ArrowController.DashState dashState = ArrowController.DashState.None;
    private float remainDashCount;

    private Coroutine fovChangeCoroutine;
    private Coroutine volumeChangeCoroutine;
    private Coroutine smoothValueCoroutine;

    private void Start()
    {
        cam = GetComponent<Camera>();
        ApplyViewImmediate();

        defaultFov = cam.fieldOfView;

        controller.OnDashStart += OnDash;
        controller.OnHitEnemy += OnHitEnemy;
        controller.OnDashStateChanged += OnArrowDashStateChanged;

        currentSmooth = SmoothTime;

    }

    private void LateUpdate()
    {
        if (target == null) return;

    
        //cam.fieldOfView = Mathf.Lerp(DashFov, DefaultFov, remainDashCount / RemainDashTime);
        //remainDashCount += Time.deltaTime;
    
        

        Vector3 targetWorldPos = target.TransformPoint(Offset);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetWorldPos,
            ref _currentVelocity,
            currentSmooth
        );

        Vector3 lookTarget = target.transform.position;
        transform.LookAt(lookTarget);
        Debug.Log($"Default fov  : {defaultFov}");
    }

    private void ApplyViewImmediate()
    {
        transform.position = target.TransformPoint(Offset);
        transform.LookAt(target.transform.position);
        _currentVelocity = Vector3.zero;
    }

    private void OnDash()
    {
        
        remainDashCount = 0;

    }

    private void OnHitEnemy()
    {
 


    }

    private void OnArrowDashStateChanged(ArrowController.DashState state)
    {
        
       
        switch (state)
        {
            //차징상태 진입
            case ArrowController.DashState.Charging:
                if (fovChangeCoroutine != null) StopCoroutine(fovChangeCoroutine);
                fovChangeCoroutine = StartCoroutine(FovChangeSmooth(defaultFov, ChargingFov, NormalToChargeTime));

                if (volumeChangeCoroutine != null) StopCoroutine(volumeChangeCoroutine);
                volumeChangeCoroutine = StartCoroutine(VolumeChangeSmooth(true));

                if(smoothValueCoroutine != null) StopCoroutine(smoothValueCoroutine);
                smoothValueCoroutine = StartCoroutine(SmoothValueLerp(SmoothTime, ChargingSmooth, NormalToChargeTime));

                break;

            case ArrowController.DashState.Dash:
                Debug.Log("대쉬상태 진입");
                if (fovChangeCoroutine != null) StopCoroutine(fovChangeCoroutine);
                fovChangeCoroutine  =  StartCoroutine(FovChangeSmooth(ChargingFov, DashFov, ChargeToDashTime));

                if (volumeChangeCoroutine != null) StopCoroutine(volumeChangeCoroutine);
                volumeChangeCoroutine = StartCoroutine(VolumeChangeSmooth(false));

                if (smoothValueCoroutine != null) StopCoroutine(smoothValueCoroutine);
                smoothValueCoroutine = StartCoroutine(SmoothValueLerp(ChargingSmooth, SmoothTime, ChargeToDashTime));
                
                currentSmooth = SmoothTime;

                break;
            case ArrowController.DashState.Cooldown:
                if (fovChangeCoroutine != null) StopCoroutine(fovChangeCoroutine);
                fovChangeCoroutine  =  StartCoroutine(FovChangeSmooth(DashFov, defaultFov, DashToNormalTime));
                break;


        }
    }

    IEnumerator FovChangeSmooth(float originFov, float targetFov, float time)
    {
        var count = 0f;
        while(count < time)
        {
            count += Time.deltaTime;
            currentFov = Mathf.Lerp(originFov, targetFov, count / time);
            
            yield return null;
        }
        
    }

    IEnumerator VolumeChangeSmooth(bool charge)
    {
        var count = 0f;
        var time = charge ? InChargeTime : OutChargeTime;
        var originWeight = charge ? 0f : 1f;
        var targetWeight = charge ? 1f : 0f;
        
        while(count < time)
        {
            count += Time.deltaTime;
            ChargeVolume.weight = Mathf.Lerp(originWeight, targetWeight, count / time);
            yield return null;
        }
    }

    IEnumerator SmoothValueLerp(float origin, float target, float time)
    {
        var count = 0f;
        while (count < time)
        {
            count += Time.unscaledDeltaTime;
            currentSmooth = Mathf.Lerp(origin, target, count / time);
            yield return null;
        }
    }





}