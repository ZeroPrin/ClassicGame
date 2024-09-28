using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform _inventortyPanel;

    [SerializeField] private GameObject _itemUI;

    private List<ItemUI> _itemsUI = new List<ItemUI>();

    private Inventory _inventory;

    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    public void Awake()
    {
        _inventory.OnInventoryUpdated += UpdateInventoryPanel;
    }

    void UpdateInventoryPanel()
    {
        ClearInventoryPanel();

        for (int i = 0; i < _inventory.InventoryObjects.Length; i ++)
        {
            InventoryObject item = _inventory.InventoryObjects[i];
            int count = _inventory.ObjectsCount[i];

            if (item != null)
            {
                GameObject newObject = Instantiate(_itemUI, Vector3.zero, Quaternion.identity, _inventortyPanel);

                ItemUI tmp_item = newObject.GetComponent<ItemUI>();
                tmp_item.Name.SetText(item.Name);
                tmp_item.Count.SetText($"{count}");
                tmp_item.Icon.sprite = item.Icon;
                tmp_item.Prefab = item.Prefab;
                tmp_item.Index = i;

                _itemsUI.Add(tmp_item);
            }
        }
    }

    void ClearInventoryPanel() 
    {
        foreach (ItemUI item in _itemsUI) 
        {
            Destroy(item.gameObject);
        }

        _itemsUI.Clear();
    }
}
