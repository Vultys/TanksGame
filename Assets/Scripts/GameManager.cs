using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] public PlayerController _playerController;

    [Inject] public EnemyController _enemyController;
}
