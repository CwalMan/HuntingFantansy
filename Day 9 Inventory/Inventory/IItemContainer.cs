public interface IItemContainer // go to ItemContainer Script
{
    bool AddItem(ItemSO item);
    bool CanAddItem(ItemSO item, int amount = 1);
    ItemSO RemoveItem(string itemID);
    bool RemoveItem(ItemSO item);
    int ItemCount(string itemID);

    void Clear();
}
