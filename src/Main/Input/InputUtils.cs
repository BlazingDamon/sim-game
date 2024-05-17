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
            // TODO: this input handler should incorporate the current menu, if any are active
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Escape:
                    GameGlobals.IsGameRunning = false;
                    break;
                case ConsoleKey.P:
                    GameGlobals.IsSimulationRunning = !GameGlobals.IsSimulationRunning;
                    GameDebugLogger.WriteLog($"Game {(GameGlobals.IsSimulationRunning ? "unpaused" : "paused")}.");
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
