using UnityEngine;

public class Enemy_Tele : EnemyBase
{
    public float dashInterval = 3f;
    private float lastDashTime = 0f;
    public float minDashDistance = 0.5f;
    public float maxDashDistance = 2.0f;
    public float range = 1.0f;
    protected override void Movement()
    {
        if (Time.time - lastDashTime >= dashInterval)
        {
            Dash();
            lastDashTime = Time.time;
        }
    }

    protected override void OnHit(Collider other)
    {
        OnDamage();
    }

    protected override void Init()
    {
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        lastDashTime = Time.time;
    }

    protected override void OnDamage()
    {
        ApplyScore(40);
        this.gameObject.SetActive(false);
    }

    private void Dash()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float dashDistance = Random.Range(minDashDistance, maxDashDistance);
        transform.position += direction * dashDistance;
        transform.position += transform.right * Random.Range(-range, range);
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        if (Vector3.Distance(transform.position, target.position) < maxDistance)
        {
            transform.position = target.position - (target.position - transform.position).normalized * maxDistance;
        }   
    }
}
