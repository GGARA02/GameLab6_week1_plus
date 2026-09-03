using UnityEngine;
using UnityEngine.AI;

public class Enemy_Fast : EnemyBase
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    public void SetData(Transform target, IScoreHandler scoreHandler, float speed)
    {
        SetData(target, scoreHandler);
        this.speed = speed;
        navMeshAgent.speed = 3.5f * 5f;
    }

    protected override void Movement()
    {
        if (Vector3.Distance(this.transform.position, target.position) >= maxDistance)
            navMeshAgent.destination = target.position;
        else
            navMeshAgent.destination = transform.position;
    }

    protected override void OnDamage()
    {
        ApplyScore(20);
        this.gameObject.SetActive(false);
    }

    protected override void Init()
    {

    }

    protected override void OnHit(Collider other)
    {
        OnDamage();
    }
}
