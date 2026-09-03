using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPool : MonoBehaviour
{
    [SerializeField] private List<EnemyBase> enemyPrefabs;
    [SerializeField] private int poolSize;

    private List<List<GameObject>> pools;

    private Transform target;
    private IScoreHandler scoreHandler;

    public void Initialize(Transform target, IScoreHandler scoreHandler)
    {
        this.target = target;
        this.scoreHandler = scoreHandler;

        pools = new List<List<GameObject>>();
        foreach (var prefab in enemyPrefabs)
        {
            var pool = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                pool.Add(SpawnEnemy(prefab));
            }
            pools.Add(pool);
        }
    }

    private GameObject SpawnEnemy(EnemyBase enemyBase)
    {
        var prefab = Instantiate(enemyBase.gameObject);
        prefab.SetActive(false);
        if (enemyBase.GetType() == typeof(Enemy_Basic))
        {
            Enemy_Basic enemy = prefab.GetComponent<Enemy_Basic>();
            enemy.SetData(target, scoreHandler);
        }
        else if (enemyBase.GetType() == typeof(Enemy_Head))
        {
            Enemy_Head enemy = prefab.GetComponent<Enemy_Head>();
            enemy.SetData(target, scoreHandler, 2);
        }
        else if (enemyBase.GetType() == typeof(Enemy_Tele))
        {
            Enemy_Tele enemy = prefab.GetComponent<Enemy_Tele>();
            enemy.SetData(target, scoreHandler);
        }
        else if(enemyBase.GetType() == typeof(Enemy_Fast))
        {
            Enemy_Fast enemy = prefab.GetComponent<Enemy_Fast>();
            enemy.SetData(target, scoreHandler, 0.05f);
        }
        else if (enemyBase.GetType() == typeof(Enemy_Nav))
        {
            Enemy_Nav enemy = prefab.GetComponent<Enemy_Nav>();
            enemy.SetData(target, scoreHandler);
        }
        return prefab;
    }

    public GameObject Pop(int index)
    {
        if (index >= pools.Count)
            return null;
        var selectedPool = pools[index];
        foreach (var item in selectedPool)
        {
            if (!item.activeInHierarchy)
                return item;
        }

        var newbie = SpawnEnemy(enemyPrefabs[index]);
        newbie.SetActive(false);
        selectedPool.Add(newbie);
        return newbie;
    }

    public GameObject GetObject()
    {
        return Pop(0);
    }

    public GameObject GetObject2()
    {
        return Pop(1);
    }

    public bool IsAllInactive()
    {

        foreach(var pool in pools)
        {
            foreach (var item in pool)
            {
                if (item.activeInHierarchy)
                    return false;
            }
        }
        
        return true;

    }
}
