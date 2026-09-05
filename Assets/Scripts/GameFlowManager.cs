using Unity.VisualScripting;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField]
    private ArrowController arrowController;
    [SerializeField]
    private ArrowCamera arrowCamera;
    [SerializeField]
    private SkyManager skyManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //이니셜 및 이벤트 구독합시다 
        arrowController.Initialize();
        arrowCamera.Initialize(arrowController);
        skyManager.Initialize();
        arrowController.OnLightUp += skyManager.CityLightUp;
        arrowController.OnGameOver += GameOver;
        skyManager.OnGameClear += GameClear;
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
    }

    private void GameClear()
    {
            
    }
}
