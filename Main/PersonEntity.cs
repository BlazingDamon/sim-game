namespace Main;

internal class PersonEntity : ISimulated
{
    public long AgeInSeconds { get; set; }
    public bool IsAlive { get; set; } = true;

    public void RunSimulationFrame()
    {
        if (IsAlive)
        {
            AgeInSeconds += GameConfig.TimePerFrameInSeconds;

            IsAlive = CheckHealth();
        }
    }

    private bool CheckHealth()
    {
        int healthCheckRoll = GameRandom.NextIntNormalized(0, 350);
        bool isAlive = healthCheckRoll > (AgeInSeconds / GameConstants.SECONDS_IN_YEAR);
        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"Person died. Age: {AgeInSeconds / GameConstants.SECONDS_IN_YEAR}. HealthCheckRoll: {healthCheckRoll}.");
        }

        return isAlive;
    }
}
