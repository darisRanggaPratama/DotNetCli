namespace GamesCLI;

public interface IInventoryHolder
{
    List<Item> Inventory { get; }
    void AddItem(Item item);
    bool UseItem(string itemName);
}
