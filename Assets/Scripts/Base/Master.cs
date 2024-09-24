using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private HandController _handController;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerStatsUI _playerStatsUI;

    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryUI _inventoryUI;

    public void Awake()
    {
        InitializeAll();
    }

    private void OnDestroy()
    {
        DeinitializeAll();
    }

    public void InitializeAll()
    {
        _playerController.Initialize();
        _inventoryUI.Initialize();
        _inventory.Initialize();
        _playerStatsUI.Initialize();
        _playerStats.Initialize();
    }

    public void DeinitializeAll()
    {
        _playerController.Deinitialize();
        _inventory.Deinitialize();
    }
}
