using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private Transform _projectileSpawnPoint;

    [SerializeField] private Projectile _projectilePrefab;

    [SerializeField] private Collider2D _playerCollider;

    [Header("Settings")]
    [SerializeField] private float _projectileSpeed = 30f;

    [SerializeField] private float _fireRate = 0.75f;

    [SerializeField] private int _startProjectilesPoolSize = 3;

    [SerializeField] private int _maxProjectilesPoolSize = 15;

    private ObjectPool<Projectile> _projectilesPool;

    private bool _shouldFire;

    private float _previousFireTime;

    private void OnEnable() 
    {
        _inputReader.FireEvent += HandleFire;    
    }

    private void OnDisable() 
    {
        _inputReader.FireEvent -= HandleFire;
    }

    private void Start() 
    {
        CreateProjectilesPool();    
    }

    private void Update() 
    {
        if(!_shouldFire) return;

        if(Time.time < (1 / _fireRate) + _previousFireTime) return;

        SpawnProjectile();

        _previousFireTime = Time.time;
    }

    public void ReleaseBulletFromPool(Projectile projectile)
    {
        _projectilesPool.Release(projectile);
    }

    private void SpawnProjectile()
    {
        Projectile projectile = _projectilesPool.Get();
        projectile.Init(this);

        Physics2D.IgnoreCollision(_playerCollider, projectile.GetComponent<Collider2D>());

        if(projectile.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = rigidbody.transform.up * _projectileSpeed;
        }
    }

    private void HandleFire(bool shouldFire)
    {
        _shouldFire = shouldFire;
    }

    private void CreateProjectilesPool()
    {
        _projectilesPool = new ObjectPool<Projectile>(CreateNewProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, false, _startProjectilesPoolSize, _maxProjectilesPoolSize);
    }

    private Projectile CreateNewProjectile() => Instantiate(_projectilePrefab);

    private void OnGetProjectile(Projectile projectile)
    {
        projectile.transform.position = _projectileSpawnPoint.position;
        projectile.transform.up = _projectileSpawnPoint.up;
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        if(projectile.TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.velocity = Vector2.zero;
        }

        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile) => Destroy(projectile);
}
