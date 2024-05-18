namespace Main.Items;
internal static class ItemSearcher
{
    public static int GetItemCount<T>() where T : Item =>
        GameGlobals.CurrentGameState.GlobalInventory.Count(x => x is T);

    public static bool CheckItemCountIsAtLeast<T>(int checkCount) where T : Item
    {
        var currentCount = 0;
        GameGlobals.CurrentGameState.GlobalInventory
            .TakeWhile(x =>
            {
                if (x is T)
                    currentCount += 1;
                return currentCount < checkCount;
            }).ToList();

        return currentCount >= checkCount;
    }

    public static bool TryUseItem<T>(int count = 1) where T : Item
    {
        if (!CheckItemCountIsAtLeast<T>(count))
            return false;

        int usedItems = 0;
        while (usedItems < count)
        {
            Item foundItem = GameGlobals.CurrentGameState.GlobalInventory.First(x => x is T);
            usedItems++;
            GameGlobals.CurrentGameState.GlobalInventory.Remove(foundItem);
        }

        return true;
    }
}
