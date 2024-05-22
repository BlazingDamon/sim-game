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
                        Helpers.RunMethodManyTimes(GameSimulator.RunFrame, GameGlobals.GameSpeed);
                    }
                    GameBaker.BakeAll();
                    GameRender.Render();
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
            Console.WriteLine(exception?.ToString() ?? "Statue Hoard was closed.");
            Console.CursorVisible = true;
        }
    }

    private static void Initialize()
    {
        // Do any game init things here
        GameGlobals.IsGameRunning = true;
        GameGlobals.IsSimulationRunning = true;
        GameGlobals.UserPrefersSimulationRunning = true;
    }

    private static void SplashScreen()
    {
        string[] splashText =
            [
                "Welcome to Statue Hoard!",
                "",
                "You are the leader of a small group of travelers looking to start a settlement.",
                "Build farms to produce food, lumber mills to produce wood, and quarries for stone.",
                "Be careful to not run out of food! If you stockpile enough food, you might attract",
                "extra travelers to join your village. As all travelers know, the best way to become",
                "wealthy is to create and collect statues. Once you have stockpiled some wood and stone,",
                "build statue workshops to begin creating statues from wood and stone.",
                "",
                "In 180 days, your collection of statues should be the envy of the land!",
                "",
                "Good luck!",
                "",
                "",
                "Press [enter] to begin..."
            ];

        int textWidth = splashText.Select(x => x.Length).Max();
        int textHeight = splashText.Length;
        int centerTextHeight = textHeight / 2;
        int centerConsoleWidth = Console.WindowWidth / 2;
        int centerConsoleHeight = Console.WindowHeight / 2;

        Console.Clear();
        for (int i = 0; i < splashText.Length; i++)
        {
            string lineOfText = splashText[i];
            Console.SetCursorPosition(centerConsoleWidth - lineOfText.Length / 2, centerConsoleHeight - centerTextHeight + i);
            Console.Write(lineOfText);
        }

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
