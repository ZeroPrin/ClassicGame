using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public static Master instance;

    public static PlayerController PlayerController => instance._playerController;
    public static HandController HandController => instance._handController;
    public static Inventory Inventory => instance._inventory;
    public static InventoryUI InventoryUI => instance._inventoryUI;

    [Header("Player")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private HandController _handController;

    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryUI _inventoryUI;

    public void Awake()
    {
        instance = this;

        InitializeAll();
    }

    public void InitializeAll() 
    {
        PlayerController.Initialize();
        Inventory.Initialize();
        InventoryUI.Initialize();
    }
}
