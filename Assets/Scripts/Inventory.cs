using System.Collections.Generic;
using Items;

namespace DefaultNamespace
{
    public class Inventory
    {
        public Dictionary<Item, int> items = new ();
        
        public void add(Item item, int amount) => items[item] += amount;
    }
}