using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    [SerializeField] private List<Transform> _enemySpawnPoints;

    [SerializeField] private int _enemiesCount = 5;

    private ObjectPool<Enemy> _enemyPool;

    private int _startEnemyPoolSize = 5;

    private int _maxEnemyPoolSize = 10;

    private int _tanksCount;

    private void Start() 
    {
        CreateEnemyPool(); 
        SpawnTanks(_enemiesCount);
    }

    public void ReleaseEnemyFromPool(Enemy enemy)
    {
        _enemyPool.Release(enemy);
        _tanksCount--;
        if(_tanksCount <= 0)
        {
            SpawnTanks(_enemiesCount);
        }
    }

    private void SpawnTanks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Enemy enemy = _enemyPool.Get();
            enemy.Init(this);
        }
        _tanksCount = _enemiesCount;
    }

    private void CreateEnemyPool()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateNewEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy, false, _startEnemyPoolSize, _maxEnemyPoolSize);
    }

    private Enemy CreateNewEnemy() => Instantiate(_enemyPrefab);

    private void OnGetEnemy(Enemy enemy)
    {
        enemy.transform.position = GetSpawnPoint().position;
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseEnemy(Enemy enemy) => enemy.gameObject.SetActive(false);

    private void OnDestroyEnemy(Enemy enemy) => Destroy(enemy);

    private Transform GetSpawnPoint()
    {
        return _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
    }
}
