
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public interface ISpawnUI
{
    void UpdateWaveText(int wave);
}

public struct WaveInfo
{
    public int wave;
    public int[] enemySpawnTable;

    public int[] spawnPointTable;

    public WaveInfo(int wave, int[] enemySpawnTable, int[] spawnPointTable)
    {
        this.wave = wave;
        this.enemySpawnTable = enemySpawnTable;
        this.spawnPointTable = spawnPointTable;
    }
}


[RequireComponent(typeof(EnemyPool))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnInterval = 0.5f;

    [SerializeField]
    private float spawnRate = 10f;

    [SerializeField]
    public Transform[] spawnPoints;

    private ISpawnUI spawnerUI;

    public System.Action<WaveInfo> OnWaveClear;
    public System.Action OnGameClear;

    private EnemyPool pool;

    // 현재 wave
    public int currentWave = 1;

    // 스폰 위치(원)
    public float spawnDistance = 50f;

    private bool isGameOn;

    public WaveInfo[] WaveInfos => waveInfos;

    

    private void Awake()
    {
        pool = GetComponent<EnemyPool>();
    }

    public void Initialize(Transform target, ISpawnUI spawnUI, IScoreHandler scoreHandler)
    {
        spawnerUI = spawnUI;
        pool.Initialize(target, scoreHandler);
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("스폰 포인트 없다");
        }
    }

    [ContextMenu("일단 원형으로 10개 박기")]
    private void SpawnFallBack()
    {
        GameObject prefab = new GameObject();    // 생성할 프리팹
        int count = 30;        // 생성할 개수
        float radius = 50f;
        spawnPoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            // 1. 현재 순서에 맞는 각도 계산 (라디안 단위)
            float angle = i * Mathf.PI * 2 / count;

            // 2. 삼각함수로 X, Z (또는 Y) 좌표 계산
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            // 3. 생성할 위치 설정 (스크립트가 붙은 오브젝트 중심)
            Vector3 spawnPosition = new Vector3(x, 0, z) + transform.position;

            // 4. 오브젝트 생성 및 바깥쪽을 바라보도록 회전 설정
            Quaternion spawnRotation = Quaternion.LookRotation(spawnPosition - transform.position);
            var point = Instantiate(prefab, spawnPosition, spawnRotation, this.transform);
            point.name = $"SpawnPoint_{i}";
            spawnPoints[i] = point.transform;
        }
    }


    IEnumerator SpawnEnemy()
    {
        int waveIndex = 0;
        int tableIndex = 0;

        while (isGameOn)
        {
            if (waveIndex >= waveInfos.Length)
            {
                // 웨이브 다끝남.
                    
                break;
            }
            var waveData = waveInfos[waveIndex];

            if (tableIndex >= waveData.enemySpawnTable.Length)
            {
                //다음 웨이브 넘겨
                waveIndex++;
                tableIndex = 0;
                if (waveIndex >= waveInfos.Length)
                    ;
                else
                    OnWaveClear?.Invoke(waveInfos[waveIndex]);
                yield return new WaitForSeconds(spawnRate);
                continue;
            }
            var spawnArr = waveData.enemySpawnTable;
            // 스폰 처리
            var enemyObj = pool.Pop(spawnArr[tableIndex]);
            enemyObj.SetActive(true);
            enemyObj.transform.position = ExtractSafeSpawnPoint(waveData).position;
            UnityEngine.Debug.Log($"EnemySpawn!! Name : {enemyObj.name}, Wave : {waveData.wave}, TableIndex : {tableIndex}, SpawnIndex : {spawnArr[tableIndex]}");
            // 스폰 인터벌 대기
            yield return new WaitForSeconds(spawnInterval);
            tableIndex++;
        }

        while(!pool.IsAllInactive())
        {
            yield return null;
        }
        Debug.Log("Game Clear!!");
        OnGameClear?.Invoke();
        
    }

    private Transform ExtractSafeSpawnPoint(WaveInfo info)
    {
        // 스폰 포인트 테이블
        var spawnPointTable = info.spawnPointTable;
        // 테이블 길이
        int tableLen = spawnPointTable.Length;

        // 테이블 내 랜덤 접근
        int randNum = UnityEngine.Random.Range(0, tableLen - 1);

        // 결과값으로 값 꺼냄
        int selectedIndex = spawnPointTable[randNum];

        // 만약 결과물이 포인트 배열보다 오바하면
        if (selectedIndex >= spawnPoints.Length)
        {
            // 스폰 포인트중 가장 마지막꺼로 강제 배정
            selectedIndex = spawnPoints.Length - 1;
        }
        // 결과물을 스폰 포인트에서 추출
        var result = spawnPoints[selectedIndex];

        Debug.Log($"Selected Spawn Point Index : {selectedIndex}");
        return result;
    }

    public void GameStart()
    {
        isGameOn = true;
        StartCoroutine(SpawnEnemy());
    }

    public void GameOver()
    {
        isGameOn = false;
    }

    private WaveInfo[] waveInfos = new WaveInfo[]
    {
        new WaveInfo(1,new int[] { 0,1,0,1,0,1 },new int[] { 0,1,2,3}),
        new WaveInfo(2,new int[] { 0,0,0,0,1,0,1,1 },new int[] { 0,1,2,3,4}),
        new WaveInfo(3,new int[] { 1,0,1,2,1,0 },new int[] { 0,1,2,3,4,5}),
        new WaveInfo(4,new int[] { 0,2,1,0,2,2 },new int[] { 0,1,2,3,4,5,6}),
        new WaveInfo(5,new int[] { 1,2,3,0,1,0,2,0 },new int[] { 0,1,2,3,4,5,6,7 }),
        new WaveInfo(6,new int[] { 1,2,3,3,3,0,2,0 },new int[] { 0,2,4,6,8 }),
        new WaveInfo(7,new int[] { 1,2,0,0,3,1 },new int[] { 1,2,3,4,5,6,7 }),
        new WaveInfo(8,new int[] { 2,3,1,1,0,1,0,1,1,2,0 },new int[] { 4,5,6,7 }),
        new WaveInfo(9,new int[] { 3,2,3,1,0,1,0,3,1,2,0,1,1,2,0 },new int[] { 0,4,5,6,7 }),
        new WaveInfo(10,new int[] { 1,3, 1, 0, 2, 3, 2,3,1, 2, 0, 1, 1, 1,0,3,1,2,0 },new int[] { 0,1,2,3,4,5,6,7 }),
    };
}
