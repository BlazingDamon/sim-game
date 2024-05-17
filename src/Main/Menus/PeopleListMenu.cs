using Main.Menus.Base;

namespace Main.Menus;
internal class PeopleListMenu : Menu
{

    public PeopleListMenu()
    {
        MenuTitle = "   HELP MENU";
        MenuBody =
            [
                "Close Menu: [esc]",
                "Pause: [p]",
                "Help Menu: [h]",
                "50x Speed: [.]",
                "1x Speed: [,]",
            ];
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Escape:
                GameGlobals.MenuStack.Pop();
                GameDebugLogger.WriteLog($"Help menu exited.");
                break;
            default:
                wasKeyHandled = false;
                break;
        }

        return wasKeyHandled;
    }
}
