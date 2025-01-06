using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    [SerializeField] private List<Transform> _enemySpawnPoints;

    [SerializeField] private float _enemiesCount = 5;

    private ObjectPool<Enemy> _enemyPool;

    private int _startEnemyPoolSize = 5;

    private int _maxEnemyPoolSize = 10;

    private void Start() 
    {
        CreateEnemyPool(); 
        for (int i = 0; i < _enemiesCount; i++)
        {
            Enemy enemy = _enemyPool.Get();
            enemy.Init(this);
        }
    }

    public void ReleaseEnemyFromPool(Enemy enemy)
    {
        _enemyPool.Release(enemy);
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
