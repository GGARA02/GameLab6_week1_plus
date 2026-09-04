using Unity.VisualScripting;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField]
    private ArrowController arrowController;
    private ArrowCamera arrowCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //이니셜 및 이벤트 구독합시다 
        arrowController.Initialize();
        arrowCamera.Initialize();
        HandleGameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleGameStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
