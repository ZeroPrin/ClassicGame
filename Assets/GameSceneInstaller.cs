using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerInput>().AsSingle();

        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<HandController>().FromComponentInHierarchy().AsSingle();

        Container.Bind<PlayerStats>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerStatsUI>().FromComponentInHierarchy().AsSingle();

        Container.Bind<Inventory>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InventoryUI>().FromComponentInHierarchy().AsSingle();
    }
}
