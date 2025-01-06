using System;
using UnityEngine;
using Zenject;

public class GameManagerInstaller : MonoInstaller
{
    [SerializeField] private GameObject _playerPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromNew().AsSingle();
        Container.Bind<EnemyController>().FromNew().AsSingle();

        BindPlayer();
    }

    private void BindPlayer()
    {
        var player = Container.InstantiatePrefabForComponent<GameObject>(_playerPrefab);
    }
}