using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private float _respawnTime = 1f;

    [SerializeField] private List<Transform> _playerSpawnPoints;

    private GameObject _activePlayer;

    private DestroySelfOnCollisionWithEnemy _hideOnCollision;

    private void Start() 
    {
        SpawnPlayer();
    }

    private void DestroyPlayer()
    {
        _hideOnCollision.OnHide -= DestroyPlayer;

        Destroy(_activePlayer);

        Invoke("SpawnPlayer", _respawnTime);
    }

    private void SpawnPlayer()
    {
        _activePlayer = Instantiate(_playerPrefab, _playerSpawnPoints[Random.Range(0, _playerSpawnPoints.Count)].position, Quaternion.identity);

        _hideOnCollision = _activePlayer.GetComponent<DestroySelfOnCollisionWithEnemy>();
        
        _hideOnCollision.OnHide += DestroyPlayer;
    }
}
