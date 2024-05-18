using Main.Menus;
using Main.Menus.Base;

namespace Main;

internal class InputUtils
{
    public static void PressEnterToContinue()
    {
    GetInput:
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.Enter:
                return;
            case ConsoleKey.Escape:
                GameGlobals.IsGameRunning = false;
                return;
            default:
                goto GetInput;
        }
    }

    public static void HandleUserInput()
    {
        while (Console.KeyAvailable)
        {
            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            if(GameGlobals.MenuStack.TryPeek(out Menu? menu))
            {
                var wasInputHandled = menu.HandleInput(key);
                
                if (wasInputHandled)
                {
                    continue;
                }
            }

            switch (key)
            {
                case ConsoleKey.Escape:
                    GameGlobals.MenuStack.Push(new MainPauseMenu());
                    GameGlobals.UserPrefersSimulationRunning = GameGlobals.IsSimulationRunning;
                    GameGlobals.IsSimulationRunning = false;
                    GameDebugLogger.WriteLog($"Pause menu opened.");
                    break;
                case ConsoleKey.P:
                    GameGlobals.IsSimulationRunning = !GameGlobals.IsSimulationRunning;
                    GameDebugLogger.WriteLog($"Game {(GameGlobals.IsSimulationRunning ? "unpaused" : "paused")}.");
                    break;
                case ConsoleKey.H:
                    MenuUtils.TryOpenMenu<MainHelpMenu>();
                    break;
                case ConsoleKey.L:
                    MenuUtils.TryOpenMenu<PeopleListMenu>();
                    break;
                case ConsoleKey.B:
                    MenuUtils.TryOpenMenu<BuildingMenu>();
                    break;
                case ConsoleKey.OemPeriod:
                    if (GameGlobals.GameSpeed != 50)
                    {
                        GameGlobals.GameSpeed = 50;
                        GameDebugLogger.WriteLog("Game speed: 50");
                    }
                    break;
                case ConsoleKey.OemComma:
                    if (GameGlobals.GameSpeed != 1)
                    {
                        GameGlobals.GameSpeed = 1;
                        GameDebugLogger.WriteLog("Game speed: 1");
                    }
                    break;
                case ConsoleKey.R:
                    GameDebugLogger.WriteLog("random int (0-250) normalized: " + GameRandom.NextIntNormalized(0, 250).ToString());
                    break;
                default:
                    break;
            }
        }
    }
}
