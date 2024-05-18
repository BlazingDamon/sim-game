using Main.Menus.Base;

namespace Main.Menus;
internal class PeopleListMenu : Menu
{
    public PeopleListMenu()
    {
        Layout = LayoutType.RightThird;
        MenuTitle = "   PEOPLE";
        MenuBody =
            [
                "Alice",
                "Bob",
                "Charlie"
            ];
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Escape:
                GameGlobals.MenuStack.Pop();
                GameDebugLogger.WriteLog($"People menu exited.");
                break;
            default:
                wasKeyHandled = false;
                break;
        }

        return wasKeyHandled;
    }

    public override string[] BakeMenuBody()
    {
        return base.BakeMenuBody();
    }
}
