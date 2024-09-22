using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public static Master instance;

    public static PlayerController PlayerController => instance._playerController;
    public static HandController HandController => instance._handController;
    public static PlayerStats PlayerStats => instance._playerStats;
    public static PlayerStatsUI PlayerStatsUI => instance._playerStatsUI;
    public static Inventory Inventory => instance._inventory;
    public static InventoryUI InventoryUI => instance._inventoryUI;

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
        instance = this;

        InitializeAll();
    }

    private void OnDestroy()
    {
        DeinitializeAll();
    }

    public void InitializeAll() 
    {
        PlayerController.Initialize();
        InventoryUI.Initialize();
        Inventory.Initialize();
        PlayerStatsUI.Initialize();
        PlayerStats.Initialize();
    }

    public void DeinitializeAll() 
    {
        PlayerController.Deinitialize();
        Inventory.Deinitialize();
        
    }
}
