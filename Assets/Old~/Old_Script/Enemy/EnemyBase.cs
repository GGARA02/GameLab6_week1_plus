using UnityEngine;


/// <summary>
/// 점수 및 콤보 계산할 담당자 인터페이스
/// </summary>
public interface IScoreHandler
{
    void ApplyScore(int score);
    int GetCombo();
    int GetMaxCombo();
    int GetComboLevel();
    int GetScore();
    float GetComboRemainTime();
    float GetMaxComboRemainTime();
}

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]
    protected Transform target;

    protected IScoreHandler scoreHandler;

    [SerializeField]
    // 접근 속도
    protected float speed = 0.01f;
    [SerializeField]
    // 최대 접근 거리
    protected float maxDistance = 2;


    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Movement();
    }

    public void SetData(Transform target, IScoreHandler scoreHandler)
    {
        this.scoreHandler = scoreHandler;
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            OnHit(other);
        }
    }


    protected abstract void Init();
    protected abstract void OnHit(Collider other);
    protected abstract void Movement();
    protected abstract void OnDamage();

    protected virtual void ApplyScore(int score)
    {
        scoreHandler.ApplyScore(score);
    }

    public virtual void OnAttackPlayer()
    {
        this.gameObject.SetActive(false);
    }
}
