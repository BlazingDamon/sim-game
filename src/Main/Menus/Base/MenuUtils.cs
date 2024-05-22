namespace Main.Menus.Base;
internal static class MenuUtils
{
    public static bool TryOpenMenuFromEmpty<T>() where T : Menu, new()
    {
        if (GameGlobals.MenuStack.Count == 0)
        {
            GameGlobals.MenuStack.Push(new T());
            GameDebugLogger.WriteLog($"{typeof(T)} menu opened.");
            return true;
        }

        return false;
    }

    public static bool TryOpenMenuNoDuplicates<T>() where T : Menu, new()
    {
        if (GameGlobals.MenuStack.Count == 0 || !GameGlobals.MenuStack.Any(x => x is T))
        {
            GameGlobals.MenuStack.Push(new T());
            GameDebugLogger.WriteLog($"{typeof(T)} menu opened.");
            return true;
        }

        return false;
    }
}
