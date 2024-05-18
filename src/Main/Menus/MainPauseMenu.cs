using Main.Menus.Base;

namespace Main.Menus;
internal class MainPauseMenu : Menu
{
    public MainPauseMenu()
    {
        MenuTitle = "   MENU";
        MenuBody =
            [
                "Close Menu: [esc]",
                "Exit Game: [0]",
            ];
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        switch (pressedKey)
        {
            case ConsoleKey.Escape:
                GameGlobals.MenuStack.Pop();
                GameGlobals.IsSimulationRunning = GameGlobals.UserPrefersSimulationRunning;
                GameDebugLogger.WriteLog($"Pause menu exited.");
                break;
            case ConsoleKey.D0:
                GameGlobals.IsGameRunning = false;
                break;
            default:
                break;
        }

        return true;
    }
}
