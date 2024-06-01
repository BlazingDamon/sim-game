using Main.DebugUtils;
using Main.Menus.Base;

namespace Main.Menus;
internal class MainDebugMenu : Menu
{
    public MainDebugMenu()
    {
        MenuTitle = "   DEBUG MENU";
        var s = GameDebugStats.GetAverageFrameTimeStats(GameConfig.TargetFramerate * 10);
        MenuBody = 
            [
                "10 second averages",
                "",
                $"SimulationTimeInMilliseconds: {s.SimulationTimeInNanoseconds / 1_000_000f:0.00}",
            ];
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Escape:
                GameGlobals.MenuStack.Pop();
                GameDebugLogger.WriteLog($"Debug menu exited.");
                break;
            default:
                break;
        }

        return wasKeyHandled;
    }
}
