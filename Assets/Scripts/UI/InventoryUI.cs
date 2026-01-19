using System.Collections.Generic;
using DefaultNamespace.Managers;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public ItemUI itemUIPrefab;
    public Dictionary<string, ItemUI> itemsUiInstances = new ();

    private void Start()
    {
        GameManager.Instance.inventoryManager.OnItemQuantityChanged.AddListener(UpdateQuantity);
        GameManager.Instance.inventoryManager.OnItemAdded.AddListener(AddItemInventory);
        GameManager.Instance.inventoryManager.OnItemRemoved.AddListener(RemoveItemInventory);
    }

    private void AddItemInventory(Item item, int quantity)
    {
        var instance = Instantiate(itemUIPrefab, transform);
        instance.name = $"UI_ITEM_{item.Name}";
        itemsUiInstances.Add(item.Name, instance);
        instance.setItem(item, quantity);
    }
    
    private void RemoveItemInventory(string name)
    {
        if (!itemsUiInstances.ContainsKey(name)) return;
        Destroy(itemsUiInstances[name]);
    }

    private void UpdateQuantity(string name, int quantity)
    {
        if (!itemsUiInstances.ContainsKey(name)) return;
        itemsUiInstances[name].updateQuantity(quantity);
    }
}
