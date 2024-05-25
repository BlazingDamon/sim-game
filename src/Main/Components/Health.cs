using Main.CoreGame.Base;

namespace Main.Components;
internal class Health : IGameComponent
{
    public long AgeInSeconds { get; set; }
    public bool IsAlive { get; set; } = true;

    public long AgeInYears { get => AgeInSeconds / GameConstants.SECONDS_IN_YEAR; }

    public int MaxHealth { get; set; } = 100;
    public int HealthPoints { get; set; } = 100;

    public bool IsEntityAgeDayPassedSinceLastFrame() =>
        ((AgeInSeconds / GameConstants.SECONDS_IN_DAY) - ((AgeInSeconds - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_DAY)) > 0;

    public bool IsEntityAgeMonthPassedSinceLastFrame() =>
        ((AgeInSeconds / GameConstants.SECONDS_IN_MONTH) - ((AgeInSeconds - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_MONTH)) > 0;

    public bool IsEntityAgeYearPassedSinceLastFrame() =>
        ((AgeInSeconds / GameConstants.SECONDS_IN_YEAR) - ((AgeInSeconds - GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_YEAR)) > 0;
}
