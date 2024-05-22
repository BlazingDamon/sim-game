namespace Main;

internal class GameConfig
{
    public static int TargetFramerate = 30;
    public static int TimePerFrameInSeconds = GameConstants.SECONDS_IN_DAY / TargetFramerate / 30;

    public static int PreferredConsoleWidth = 144;
    public static int PreferredConsoleHeight = 44;
}
