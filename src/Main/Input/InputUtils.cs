using Main.DebugUtils;
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

            if (GameGlobals.MenuStack.TryPeek(out Menu? menu))
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
                case ConsoleKey.Oem2:
                    GameGlobals.IsSimulationRunning = !GameGlobals.IsSimulationRunning;
                    GameDebugLogger.WriteLog($"Game {(GameGlobals.IsSimulationRunning ? "unpaused" : "paused")}.");
                    break;
                case ConsoleKey.H:
                    MenuUtils.TryOpenMenuNoDuplicates<MainHelpMenu>();
                    break;
                case ConsoleKey.B:
                    MenuUtils.TryOpenMenuFromEmpty<BuildingMenu>();
                    break;
                case ConsoleKey.K:
                    GameGlobals.MainDisplayScrollHeight -= 5;
                    break;
                case ConsoleKey.J:
                    GameGlobals.MainDisplayScrollHeight += 5;
                    break;
                case ConsoleKey.OemPeriod:
                    if (GameGlobals.GameSpeed != 30)
                    {
                        GameGlobals.GameSpeed = 30;
                        GameDebugLogger.WriteLog("Game speed: 30");
                    }
                    break;
                case ConsoleKey.OemComma:
                    if (GameGlobals.GameSpeed != 1)
                    {
                        GameGlobals.GameSpeed = 1;
                        GameDebugLogger.WriteLog("Game speed: 1");
                    }
                    break;
                case ConsoleKey.F1:
                    GameGlobals.IsDebugModeEnabled = !GameGlobals.IsDebugModeEnabled;
                    break;
                default:
                    break;
            }
        }
    }
}
