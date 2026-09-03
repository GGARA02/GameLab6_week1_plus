using UnityEngine;

public class GameOverWallController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; // 벽 이동 속도

    public System.Action OnGameOverWallReachedPlayer; // 벽이 플레이어에 도달했을 때 호출되는 이벤트

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed); // 벽 이동 속도
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGameOverWallReachedPlayer?.Invoke();
        }
    }
}