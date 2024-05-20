namespace Main;

public interface ISimulated
{
    public void RunSimulationFrame();

    public static bool IsDayPassedSinceLastFrame(int timeOfDayInSeconds = 0) =>
        (((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - timeOfDayInSeconds) / GameConstants.SECONDS_IN_DAY) - 
        (((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - timeOfDayInSeconds) - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_DAY)) > 0;

    public bool IsMonthPassedSinceLastFrame() =>
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds / GameConstants.SECONDS_IN_MONTH) - 
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_MONTH)) > 0;

    public bool IsYearPassedSinceLastFrame() =>
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds / GameConstants.SECONDS_IN_YEAR) - 
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_YEAR)) > 0;
}
