namespace Main;

internal class GameConfig
{
    public static int TargetFramerate = 30;

    public static int TimePerFrameInSeconds = GameConstants.SECONDS_IN_MONTH / TargetFramerate;
}
