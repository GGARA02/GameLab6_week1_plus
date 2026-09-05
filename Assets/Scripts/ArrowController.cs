using System;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public enum ArrowState 
{
    None,
    BulletTime,
    Dash,
    HyperDash
}

public class ArrowController : MonoBehaviour
{
    //TODO : 쉐이더 그래프로 깜빡임 효과
    [Header("Default")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float minPitch = -20f;
    [SerializeField]
    private float maxPitch = 70f; // 피치 제한
    [SerializeField]
    private float yawLimitLog;
    [SerializeField]
    private float pitchLimitLog;
    [SerializeField]
    private float invincibleTime;
    [Header("Dash")]
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashCoolTime;
    [Header("HyperDash")]
    [SerializeField]
    private float hyperDashSpeed;
    [SerializeField]
    private float hyperDashCoolTime;
    [Header("BulletTime")]
    [SerializeField]
    private float bulletTimeSensitivity;
    [SerializeField]
    private float bulletTimeSpeed;
    [SerializeField]
    private float maxBulletTime;
    [SerializeField]
    private float bulletTimeScale;
    [SerializeField]
    private float bulletTimeWindowBreak;
    [Header("Difficult")]
    [SerializeField]
    private float difficultSpeedUp;
    private float currentSpeed;
    private float remainCoolTime; //남은 쿨타임은 조작불가능 시간과 동일하다.
    private float currentSensitivity;
    private float remainInvincibleTime;
    private float remainBulletTime;
    private float yaw;
    private float pitch;
    private ArrowState arrowState;

    public System.Action OnHitWall; //플레이어 히트처리
    public System.Action<ArrowState> OnArrowStateChange; //카메라에서 상태별 연출을 위한 이벤트
    public System.Action OnHitWindow; //플레이어가 창문을 부술때 이벤트
    public void Initialize()
    {
        currentSpeed = speed;
        remainCoolTime = 0;
        currentSensitivity = sensitivity;
        remainInvincibleTime = 0;
        remainBulletTime = maxBulletTime;
        yaw = 0;
        pitch = 0;
        arrowState = ArrowState.None;
    }


    private void Update() //여기서는 실제로 전진하는거지, 불릿타임을 깎거나, 
    {
        HandleStateLogic();
        if (remainCoolTime > 0)
        {
            remainCoolTime -= Time.deltaTime;
        }
        if (remainInvincibleTime > 0)
        {
            remainInvincibleTime -= Time.deltaTime;
        }
        if (arrowState == ArrowState.BulletTime)
        {
            remainBulletTime -= Time.deltaTime;
        }
        speed += difficultSpeedUp * Time.deltaTime;
        dashSpeed += difficultSpeedUp * Time.deltaTime * dashSpeed / speed;
        hyperDashSpeed += difficultSpeedUp * Time.deltaTime * hyperDashSpeed / speed;

        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);

        if (arrowState != ArrowState.Dash || arrowState != ArrowState.HyperDash)
        {
            HandleMouseInput();
        }
    }

    private void ChangeArrowState(ArrowState state)
    {
        arrowState = state;
        OnArrowStateChange?.Invoke(arrowState);
    }

    private void HandleStateLogic() //상태 변환 및 그에 따른 변수도 조금 바꿔주자
    {
        if (arrowState == ArrowState.None) 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeArrowState(ArrowState.Dash);
                currentSensitivity = sensitivity; //수정부
                currentSpeed = dashSpeed;
                remainCoolTime = dashCoolTime;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ChangeArrowState(ArrowState.BulletTime);
                currentSensitivity = bulletTimeSensitivity;
                currentSpeed = bulletTimeSpeed;
                Time.timeScale = bulletTimeScale;
            }
            currentSpeed = speed;
        }
        else if (arrowState == ArrowState.Dash)
        {
            if (remainCoolTime <= 0)
            {
                ChangeArrowState(ArrowState.None);
                currentSensitivity = sensitivity;
                currentSpeed = speed;
                remainCoolTime = 0;
            }
            currentSpeed = dashSpeed;
            if (remainCoolTime <= 0)
            {
                remainCoolTime = 0;
            }
        }
        else if (arrowState == ArrowState.BulletTime)
        {
            if(Input.GetKeyUp(KeyCode.LeftShift) || remainBulletTime <= 0)
            {
                ChangeArrowState(ArrowState.None);
                if (remainBulletTime <= 0)
                {
                    remainBulletTime = 0;
                }
                currentSensitivity = sensitivity;
                currentSpeed = speed;
                Time.timeScale = 1;
            }
            //else if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    ChangeArrowState(ArrowState.HyperDash);
            //    currentSensitivity = sensitivity;
            //    currentSpeed = hyperDashSpeed;
            //    remainCoolTime = hyperDashCoolTime;
            //    Time.timeScale = 1;
            //}
            remainBulletTime -= Time.deltaTime;
            if (remainBulletTime <= 0)
            {
                remainBulletTime = 0;
            }
        }
        else if (arrowState == ArrowState.HyperDash)
        {
            if (remainCoolTime <= 0)
            {
                ChangeArrowState(ArrowState.None);
                currentSensitivity = sensitivity;
                currentSpeed = speed;
                remainCoolTime = 0;
            }
            currentSpeed = hyperDashSpeed;
            if (remainCoolTime <= 0)
            {
                remainCoolTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Enemy" && remainInvincibleTime == 0)
        {
            OnHitWall?.Invoke();
        }
        else if (other.tag == "Window")
        {
            OnHitWindow?.Invoke(); //체력 증가하자고!
            remainBulletTime += bulletTimeWindowBreak;
            Mathf.Clamp(remainBulletTime, 0f, maxBulletTime);
        }
    }

    private void HandleMouseInput()
    {
        float xInput = Input.GetAxis("Mouse X");
        float yInput = Input.GetAxis("Mouse Y");
        if (xInput >= 0)
        {
            yaw += Mathf.Log((Input.GetAxis("Mouse X") * currentSensitivity) + 1, yawLimitLog);
        }
        else
        {
            yaw -= Mathf.Log((Input.GetAxis("Mouse X") * currentSensitivity * -1) + 1, yawLimitLog);
        }
        if (yInput >= 0)
        {
            pitch -= Mathf.Log((Input.GetAxis("Mouse Y") * currentSensitivity) + 1, pitchLimitLog);
        }
        else
        {
            pitch += Mathf.Log((Input.GetAxis("Mouse Y") * currentSensitivity * -1) + 1, pitchLimitLog);
        }
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
