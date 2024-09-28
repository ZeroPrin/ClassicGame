using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Player")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private PlayerStatsConfig _playerStatsConfig;

    [Header("Inventory")]
    [SerializeField] private InventoryData _savedInventoryData;

    public override void InstallBindings()
    {
        //Configs
        Container.Bind<PlayerConfig>().FromInstance(_playerConfig);
        Container.Bind<PlayerStatsConfig>().FromInstance(_playerStatsConfig);
        Container.Bind<InventoryData>().FromInstance(_savedInventoryData);

        //Player
        Player player = Container.InstantiatePrefabForComponent<Player>(_playerPrefab, _spawnPoint.position, Quaternion.identity, null);
        Container.BindInterfacesAndSelfTo<Player>().FromInstance(player).AsSingle();

        //Player Input
        Container.Bind<PlayerInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        Container.BindInterfacesAndSelfTo<HandController>().AsSingle();

        //Player Stats
        Container.Bind<PlayerStatsUI>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStats>().AsSingle();

        //Inventory
        Container.Bind<InventoryUI>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<Inventory>().AsSingle();
    }
}
