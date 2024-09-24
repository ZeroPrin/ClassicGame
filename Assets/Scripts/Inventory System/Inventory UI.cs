using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] RectTransform inventortyPanel;

    [SerializeField] GameObject itemUI;

    List<ItemUI> itemsUI = new List<ItemUI>();

    public void Initialize()
    {
        inventory.OnInventoryUpdated += UpdateInventoryPanel;
    }

    void UpdateInventoryPanel()
    {
        ClearInventoryPanel();

        for (int i = 0; i < inventory.InventoryObjects.Length; i ++)
        {
            InventoryObject item = inventory.InventoryObjects[i];
            int count = inventory.ObjectsCount[i];

            if (item != null)
            {
                GameObject newObject = Instantiate(itemUI, Vector3.zero, Quaternion.identity, inventortyPanel);

                ItemUI tmp_item = newObject.GetComponent<ItemUI>();
                tmp_item.Name.SetText(item.Name);
                tmp_item.Count.SetText($"{count}");
                tmp_item.Icon.sprite = item.Icon;
                tmp_item.Prefab = item.Prefab;
                tmp_item.Index = i;

                itemsUI.Add(tmp_item);

                //tmp_item.Initialize();
            }
        }
    }

    void ClearInventoryPanel() 
    {
        foreach (ItemUI item in itemsUI) 
        {
            Destroy(item.gameObject);
        }

        itemsUI.Clear();
    }
}
