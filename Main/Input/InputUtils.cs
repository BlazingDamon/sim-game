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
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Escape:
                    GameGlobals.IsGameRunning = false;
                    break;
                case ConsoleKey.P:
                    GameGlobals.IsSimulationRunning = !GameGlobals.IsSimulationRunning;
                    break;
                case ConsoleKey.OemPeriod:
                    GameGlobals.GameSpeed = 50;
                    GameDebugLogger.WriteLog("Game speed: 50");
                    break;
                case ConsoleKey.OemComma:
                    GameGlobals.GameSpeed = 1;
                    GameDebugLogger.WriteLog("Game speed: 1");
                    break;
                case ConsoleKey.R:
                    GameDebugLogger.WriteLog("random int (0-250) normalized: " + GameRandom.NextIntNormalized(0, 250).ToString());
                    break;
                default:
                    GameDebugLogger.WriteLog("key pressed: " + key);
                    break;
            }
        }
    }
}
