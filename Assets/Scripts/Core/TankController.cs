using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// The TankController class is responsible for managing player and enemy tank behaviors,
/// including spawning, pooling, and player respawn logic.
/// </summary>
public class TankController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private List<Transform> _spawnPoints;

    [Header("Settings")]
    [SerializeField] private float _playerRespawnTime = 1f;
    [SerializeField] private int _initialEnemyCount = 5;
    [SerializeField] private int _maxEnemyCount = 10;

    private GameObject _activePlayer;
    private ObjectPool<Enemy> _enemiesPool;
    private DestroySelfOnCollisionWithEnemy _playerDestroy;
    private int _currentEnemyCount;
    private SortedSet<int> _usedSpawns;

    private void Awake() 
    {
        _usedSpawns = new SortedSet<int>();    
    }

    private void Start() 
    {
        InitializePlayer();
        InitializeEnemyPool(); 
        SpawnEnemies(_initialEnemyCount);
    }

    /// <summary>
    /// Initializes player object by spawning the player at a random spawn point.
    /// </summary>
    private void InitializePlayer()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// Initializes the enemy pool with an initial size and maximum limit.
    /// </summary>
    private void InitializeEnemyPool()
    {
        _enemiesPool = new ObjectPool<Enemy>(CreateNewEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy, false, _initialEnemyCount, _maxEnemyCount);
    }

    /// <summary>
    /// Spawns a new player at a random spawn point.
    /// </summary>
    private void SpawnPlayer()
    {
        _activePlayer = Instantiate(_playerPrefab, GetUnusedSpawnPoint().position, Quaternion.identity);
        _playerDestroy = _activePlayer.GetComponent<DestroySelfOnCollisionWithEnemy>();
        _playerDestroy.OnDestroy += HandlePlayerDestruction;
    }

    /// <summary>
    /// Handles the player destruction event. Initiates player respawn after the set time.
    /// </summary>
    private void HandlePlayerDestruction()
    {
        _playerDestroy.OnDestroy -= HandlePlayerDestruction;
        ResetTanksSpawns();
        Invoke(nameof(SpawnPlayer), _playerRespawnTime);
    }
    
    /// <summary>
    /// Releases an enemy back into the pool and manages the spawn point reusability.
    /// If all enemies are released, respawn them.
    /// </summary>
    /// <param name="enemy">The enemy object being released.</param>
    public void ReleaseEnemy(Enemy enemy)
    {
        _enemiesPool.Release(enemy);
        _currentEnemyCount--;

        if(_currentEnemyCount <= 0)
        {
            ResetTanksSpawns();
            SpawnEnemies(_initialEnemyCount);
        }
    }
    /// <summary>
    /// Clears the used spawns to allow for resetting tank spawn points.
    /// </summary>
    private void ResetTanksSpawns() => _usedSpawns.Clear();
    
    /// <summary>
    /// Spawns the given number of enemies by getting them from the pool and initializing them.
    /// </summary>
    /// <param name="count">The number of enemies to spawn.</param>
    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Enemy enemy = _enemiesPool.Get();
            enemy.Initialize(this);
            enemy.GetComponent<JsonSaveableEntity>().GenerateUniqueIdentifier();
        }

        _currentEnemyCount = count;
    }

    /// <summary>
    /// Creates a new enemy instance from the prefab.
    /// </summary>
    /// <returns>A new enemy object.</returns>
    private Enemy CreateNewEnemy() => Instantiate(_enemyPrefab);

    /// <summary>
    /// Called when an enemy is taken from the pool. Assigns the spawn point and activates the enemy.
    /// </summary>
    /// <param name="enemy">The enemy object being activated.</param>
    private void OnGetEnemy(Enemy enemy)
    {
        enemy.transform.position = GetUnusedSpawnPoint().position;
        enemy.gameObject.SetActive(true);
    }

     /// <summary>
    /// Called when an enemy is returned to the pool. Deactivates the enemy.
    /// </summary>
    /// <param name="enemy">The enemy object being released.</param>
    private void OnReleaseEnemy(Enemy enemy) => enemy.gameObject.SetActive(false);

    /// <summary>
    /// Called when an enemy is destroyed, cleaning up the enemy object.
    /// </summary>
    /// <param name="enemy">The enemy object being destroyed.</param>
    private void OnDestroyEnemy(Enemy enemy) => Destroy(enemy);

    /// <summary>
    /// Gets an unused spawn point from the list by ensuring it hasn't been used before.
    /// </summary>
    /// <returns>An unused spawn point.</returns>
    private Transform GetUnusedSpawnPoint()
    {
        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, _spawnPoints.Count);
        } 
        while(_usedSpawns.Contains(spawnIndex));

        _usedSpawns.Add(spawnIndex);

        return _spawnPoints[spawnIndex];
    }
}
