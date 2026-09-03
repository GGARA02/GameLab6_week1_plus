using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public int maxHp = 5;
    public int curHp = 5;

    public int hitEffectDuration = 1;


    public System.Action OnPlayerDead;
    public System.Action OnPlayerHit;
    public System.Action OnPlayerHeal;


    [SerializeField]
    private Volume hitVolume;

    private Coroutine hitEffectCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            curHp--;
            OnPlayerHit?.Invoke();

            other.TryGetComponent<EnemyBase>(out var enemy);
            
            enemy?.OnAttackPlayer();

            if (hitEffectCoroutine != null)
            {
                StopCoroutine(hitEffectCoroutine);
            }
            StartCoroutine(HitEffectSmooth());

            if (curHp <= 0)
            {
                OnPlayerDead?.Invoke();
            }
        }
    }

    IEnumerator HitEffectSmooth()
    {
        var time = 0f;
        while(time < hitEffectDuration)
        {
            time += Time.deltaTime;
            hitVolume.weight = Mathf.Lerp(1, 0, time / hitEffectDuration);
            yield return null;
        }
    }

    //Wave 끝날 시 해당 함수 실행
    [ContextMenu("플레이어 힐")]
    public void PlayerHeal()
    {
        if (curHp != maxHp)
        {
            curHp++;
            OnPlayerHeal?.Invoke();
        }
    }
  

}
