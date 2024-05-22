namespace Main;

public interface ISimulated
{
    public void RunSimulationFrame();

    public static bool IsDayPassedSinceLastFrame(int timeOfDayInSeconds = 0) =>
        (((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - timeOfDayInSeconds) / GameConstants.SECONDS_IN_DAY) - 
        (((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - timeOfDayInSeconds) - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_DAY)) > 0;

    public static bool IsWeekPassedSinceLastFrame(int timeOfDayInSeconds = 0, int dayOfWeek = 0) =>
        (((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - timeOfDayInSeconds - (dayOfWeek * GameConstants.SECONDS_IN_WEEK)) / GameConstants.SECONDS_IN_WEEK) -
        (((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - timeOfDayInSeconds - (dayOfWeek * GameConstants.SECONDS_IN_WEEK)) - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_WEEK)) > 0;

    public static bool IsMonthPassedSinceLastFrame(int timeOfDayInSeconds = 0, int dayOfWeek = 0, int monthOfYear = 0) =>
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds / GameConstants.SECONDS_IN_MONTH) - 
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_MONTH)) > 0;

    public static bool IsYearPassedSinceLastFrame() =>
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds / GameConstants.SECONDS_IN_YEAR) - 
        ((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_YEAR)) > 0;
}
