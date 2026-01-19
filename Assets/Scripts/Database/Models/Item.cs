using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]public class Item: ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public string ID;
    [SerializeField] public string Name;
    [SerializeField] public string Description;
        
    public Item(string ID, Sprite sprite, string name, string description)
    {
        this.sprite = sprite;
        this.ID = ID;
        Name = name;
        Description = description;
    }
}