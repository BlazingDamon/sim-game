namespace Main;

public partial class Program
{
    public static void Main()
    {
        Exception? exception = null;
        try
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(
                Math.Min(Console.LargestWindowWidth, GameConfig.PreferredConsoleWidth),
                Math.Min(Console.LargestWindowHeight, GameConfig.PreferredConsoleHeight));

            Initialize();
            SplashScreen();
            LoadFirstScenario();
            LoadFirstScene();
            while (GameGlobals.IsGameRunning)
            {
                InputUtils.HandleUserInput();
                if (GameGlobals.IsGameRunning)
                {
                    if (GameGlobals.IsSimulationRunning)
                    {
                        // run sim logic here
                        Helpers.RunMethodManyTimes(GameSimulator.RunFrame, GameGlobals.GameSpeed);
                    }
                    GameBaker.BakeAll();
                    GameRender.RenderWorldMapView();
                    SleepAfterRender();
                }
            }
        }
        catch (Exception e)
        {
            exception = e;
            throw;
        }
        finally
        {
            Console.Clear();
            Console.WriteLine(exception?.ToString() ?? "SimGame was closed.");
            Console.CursorVisible = true;
        }
    }

    private static void Initialize()
    {
        // Do any game init things here
        GameGlobals.IsGameRunning = true;
        GameGlobals.IsSimulationRunning = true;
    }

    private static void SplashScreen()
    {
        Console.Clear();
        Console.SetCursorPosition(Console.WindowWidth / 2 - 12, Console.WindowHeight / 2 - 2);

        Console.Write("    SimGame v0.0.0");
        Console.SetCursorPosition(Console.WindowWidth / 2 - 12, Console.WindowHeight / 2);
        Console.Write("Press [enter] to begin...");
        InputUtils.PressEnterToContinue();
    }

    private static void LoadFirstScenario()
    {
        var demoScenario = new DemoScenario();
        demoScenario.Initialize();
    }

    private static void LoadFirstScene()
    {
        GameGlobals.CurrentGameState.CurrentScene = new OpeningScene();
    }

    private static void SleepAfterRender()
    {
        // frame rate control
        DateTime now = DateTime.Now;
        TimeSpan frameDelta = TimeSpan.FromMilliseconds(1000f / GameConfig.TargetFramerate) - (now - GameGlobals.PreviousRender);
        if (frameDelta > TimeSpan.Zero)
        {
            Thread.Sleep(frameDelta);
        }
        GameGlobals.PreviousRender = DateTime.Now;
    }
}
