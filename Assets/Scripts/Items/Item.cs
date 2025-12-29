namespace Items
{
    public class Item
    {
        public readonly string ID;
        public readonly string Name;
        public readonly string Description;

        public Item(string id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}