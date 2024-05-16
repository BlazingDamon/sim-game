namespace Main;

public partial class Program
{
    public static void Main()
    {
        Exception? exception = null;
        try
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            Initialize();
            OpeningScreen();
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
                    GameBaker.Bake();
                    GameRender.RenderWorldMapView(_defaultMapText);
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

    private static void Initialize()
    {
        // Do any game init things here
        GameGlobals.IsGameRunning = true;
        GameGlobals.IsSimulationRunning = true;

        InitializePeopleList();
    }
}

// TODO: this code should not live in Program
public partial class Program
{
    private static readonly string[] _defaultMapText =
    [
        "Test a key: [anykey]",
        "Pause: [p]",
        "50x Speed: [.]",
        "1x Speed: [,]",
        "Test random int generation: [r]",
        "Quit: [escape]",
    ];

    private static void OpeningScreen()
    {
        Console.Clear();
        Console.SetCursorPosition(Console.LargestWindowWidth / 2 - 12, Console.LargestWindowHeight / 2 - 2);

        Console.Write("    SimGame v0.0.0");
        Console.SetCursorPosition(Console.LargestWindowWidth / 2 - 12, Console.LargestWindowHeight / 2);
        Console.Write("Press [enter] to begin...");
        InputUtils.PressEnterToContinue();
    }

    private static void InitializePeopleList()
    {
        GameGlobals.CurrentGameState.SimulatedEntities.AddRange(
            new List<ISimulated>
            {
                new PersonEntity { AgeInSeconds = 86400000L * 13 },
                new PersonEntity { AgeInSeconds = 86400000L * 19 },
                new PersonEntity { AgeInSeconds = 86400000L * 25 },
            });
    }
}
