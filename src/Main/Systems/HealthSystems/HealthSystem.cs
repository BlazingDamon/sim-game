using Main.Components;
using Main.CoreGame.Base;

namespace Main.Systems.HealthSystems;
internal class HealthSystem : GameSystem
{
    public HealthSystem() : base(typeof(Health)) { }

    public override void RunSimulationFrame()
    {
        foreach (var healthPair in GetComponents<Health>())
        {
            Health health = healthPair.Get<Health>();

            if (health.IsAlive)
            {
                health.AgeInSeconds += GameConfig.TimePerFrameInSeconds;

                if (health.IsEntityAgeDayPassedSinceLastFrame())
                {
                    health.HealthPoints = Math.Min(health.MaxHealth, health.HealthPoints + 2);

                    health.IsAlive = HealthCheck(health, healthPair.EntityId);
                }

                if (health.IsEntityAgeMonthPassedSinceLastFrame())
                {
                    health.IsAlive = HealthCheckAge(health, healthPair.EntityId);
                }
            }
        }
    }

    private bool HealthCheck(Health health, ulong entityId)
    {
        int healthCheckRoll = GameRandom.NextInt(30);
        bool isAlive = healthCheckRoll < health.HealthPoints && health.HealthPoints > 0;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"Person died. Health: {health.HealthPoints}. HealthCheckRoll: {healthCheckRoll}.");
            GameGlobals.CurrentGameState.GameLogger.WriteLog($"A person has died in poor health, at age {health.AgeInYears}.");
            DoDeath(entityId);
        }

        return isAlive;
    }

    private bool HealthCheckAge(Health health, ulong entityId)
    {
        int ageCheckRoll = GameRandom.NextIntNormalized(0, 250);
        bool isAlive = ageCheckRoll > health.AgeInYears;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"A person has died of natural causes, at age {health.AgeInYears}. AgeCheckRoll: {ageCheckRoll}.");
            GameGlobals.CurrentGameState.GameLogger.WriteLog($"A person has died of natural causes, at age {health.AgeInYears}.");
            DoDeath(entityId);
        }

        return isAlive;
    }

    private void DoDeath(ulong entityId)
    {
        Job? job = GameGlobals.CurrentGameState.Components.GetGameComponent<Job>(entityId);
        job?.CurrentJob?.Unassign(job);
        GameGlobals.CurrentGameState.Components.DeleteComponent<Job>(entityId);
        GameGlobals.CurrentGameState.Components.DeleteComponent<Hunger>(entityId);
    }
}
