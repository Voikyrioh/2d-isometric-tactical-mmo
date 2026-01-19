using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Inventory: MonoBehaviour
    {
        public Dictionary<string, int> items { get; } = new ();
        
        public UnityEvent<string, int> OnItemQuantityChanged { get; private set; }
        public UnityEvent<Item, int> OnItemAdded { get; private set; }
        public UnityEvent<string> OnItemRemoved { get; private set; }
        
        private void Awake()
        {
            OnItemQuantityChanged = new UnityEvent<string, int>();
            OnItemAdded = new UnityEvent<Item, int>();
            OnItemRemoved = new UnityEvent<string>();
        }

        public void Add(Item item, int amount)
        {
            if (items.ContainsKey(item.ID))
            {
                items[item.ID] += amount;
                OnItemQuantityChanged.Invoke(item.Name, items[item.ID]);
            }
            else
            {
                items.Add(item.ID, amount);
                OnItemAdded.Invoke(item, amount);
            }
        }
        
        public void Remove(Item item)
        {
            if (!items.ContainsKey(item.ID)) return;
            items.Remove(item.ID);
            OnItemRemoved.Invoke(item.Name);
        }
        
        public void Sub(Item item, int amount)
        {
            if (!items.ContainsKey(item.ID)) return;
            items[item.ID] -= amount;
            if (items[item.ID] <= 0) Remove(item);
            else OnItemQuantityChanged.Invoke(item.Name, items[item.ID]);
        }
    }
}