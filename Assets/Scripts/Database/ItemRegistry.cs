using System.Collections.Generic;
using UnityEngine;

public class ItemRegistry: ScriptableObject
{
    private Dictionary<string, Item> registry;
    [SerializeField] private Sprite defaultSprite;
    
    private void Start()
    {
        registry = new Dictionary<string, Item>();
    }

    public Item GetItem(string id)
    {
        var item = registry.GetValueOrDefault(id, new Item($"ITEM_ERROR_{id}", defaultSprite, id, "ERROR"));
        return item;
    }
}
