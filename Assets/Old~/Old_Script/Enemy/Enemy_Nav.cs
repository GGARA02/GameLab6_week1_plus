using UnityEngine;
using UnityEngine.AI;

public class Enemy_Nav : EnemyBase
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    protected override void Movement()
    {
        if (Vector3.Distance(this.transform.position, target.position) >= maxDistance)
            navMeshAgent.destination = target.position;
        else
            navMeshAgent.destination = transform.position;
    }

    protected override void OnDamage()
    {
        ApplyScore(10);
        this.gameObject.SetActive(false);
    }

    protected override void Init()
    {

    }

    public void SetData(Transform target, IScoreHandler scoreHandler, float speed)
    {
        SetData(target, scoreHandler);
        this.speed = speed;
    }


    protected override void OnHit(Collider other)
    {
        OnDamage();
    }
}
