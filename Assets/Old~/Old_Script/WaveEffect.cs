using System.Collections;
using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    [SerializeField] private Transform effectDom;
    [SerializeField] private GameObject spawnPointEffect;
    [SerializeField] private float effectMaxSize;
    [SerializeField] private float effectDuration;
    [SerializeField] private float spawnPointEffectDuration;

    private EnemySpawner spawner;
    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
        //이벤트 등록 해주기.
        //OnWaveChanged();
    }

    public void OnWaveChanged(WaveInfo waveInfo)
    { 
        var waveInfo1 = spawner.WaveInfos[spawner.currentWave - 1];

         StartCoroutine(WaveEffectCoroutine());
    }

    IEnumerator WaveEffectCoroutine()
    {
        // Wave effect logic here
        //effectDom.gameObject.SetActive(true);
        //var timer = 0f;
        //while (timer < effectDuration)
        //{
        //    timer += Time.deltaTime;
        //    var scale = Mathf.Lerp(0, effectMaxSize, timer / effectDuration);
        //    effectDom.localScale = new Vector3(scale, scale, scale);
        //    yield return null;
        //}
        ////스폰 위치에 빔 세우기.
        //effectDom.gameObject.SetActive(false);
        var spawnEffects = new GameObject[spawner.WaveInfos[spawner.currentWave - 1].spawnPointTable.Length];
        var i = 0;
        foreach (var spawnPointIndex in spawner.WaveInfos[spawner.currentWave - 1].spawnPointTable)
        {
            var spawnPoint = spawner.spawnPoints[spawnPointIndex];
            spawnEffects[i] = Instantiate(spawnPointEffect, spawnPoint.position, Quaternion.identity);
            i++;
        }

        yield return new WaitForSeconds(spawnPointEffectDuration);

        foreach (var spawnEffect in spawnEffects)
        {
            Destroy(spawnEffect);
        }
        
    }
}
