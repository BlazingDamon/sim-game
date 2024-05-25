using Main.CoreGame.Base;

namespace Main.Systems.Health;
internal class HealthSystem : GameSystem
{
    public HealthSystem() : base(typeof(Components.Health)) { }

    public override void RunSimulationFrame()
    {
        foreach (var healthPair in _componentDictionary[typeof(Components.Health)])
        {
            Components.Health health = (Components.Health)healthPair.Component;

            if (health.IsAlive)
            {
                health.AgeInSeconds += GameConfig.TimePerFrameInSeconds;

                if (health.IsEntityAgeDayPassedSinceLastFrame())
                {
                    health.HealthPoints = Math.Min(health.MaxHealth, health.HealthPoints + 2);

                    health.IsAlive = HealthCheck(health);
                }

                if (health.IsEntityAgeMonthPassedSinceLastFrame())
                {
                    health.IsAlive = HealthCheckAge(health);
                }
            }
        }
    }

    private bool HealthCheck(Components.Health health)
    {
        int healthCheckRoll = GameRandom.NextInt(30);
        bool isAlive = healthCheckRoll < health.HealthPoints && health.HealthPoints > 0;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"Person died. Health: {health.HealthPoints}. HealthCheckRoll: {healthCheckRoll}.");
            GameGlobals.CurrentGameState.GameLogger.WriteLog($"A person has died in poor health, at age {health.AgeInYears}.");
            DoDeath();
        }

        return isAlive;
    }

    private bool HealthCheckAge(Components.Health health)
    {
        int ageCheckRoll = GameRandom.NextIntNormalized(0, 250);
        bool isAlive = ageCheckRoll > health.AgeInYears;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"A person has died of natural causes, at age {health.AgeInYears}. AgeCheckRoll: {ageCheckRoll}.");
            GameGlobals.CurrentGameState.GameLogger.WriteLog($"A person has died of natural causes, at age {health.AgeInYears}.");
            DoDeath();
        }

        return isAlive;
    }

    private void DoDeath()
    {
        // TODO implement event hook or something here
    }
}
