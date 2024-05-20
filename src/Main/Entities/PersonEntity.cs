using Main.Items.Food;
using Main.Items.Food.Base;
using Main.Systems.Jobs;
using Main.Systems.Jobs.Base;

namespace Main;

internal class PersonEntity : HasHungerEntity
{
    public Job? CurrentJob { get; set; }

    public PersonEntity()
    {
        Health = MaxHealth;
        Hunger = 0;
    }

    public override void RunSimulationFrame()
    {
        base.RunSimulationFrame();

        if (IsAlive)
        {
            if (IsEntityAgeDayPassedSinceLastFrame())
            {
                IsAlive = HealthCheck();

                if (CurrentJob is FoodForageJob && GameRandom.NextInt(2) > 0)
                {
                    GameGlobals.CurrentGameState.GlobalInventory.Add(new FarmedFoodItem());
                }
            }

            if (IsEntityAgeMonthPassedSinceLastFrame())
            {
                IsAlive = HealthCheckAge();
            }
        }
    }

    private bool HealthCheck()
    {
        int healthCheckRoll = GameRandom.NextInt(30);
        bool isAlive = healthCheckRoll < Health && Health > 0;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"Person died. Health: {Health}. HealthCheckRoll: {healthCheckRoll}.");
        }

        return isAlive;
    }

    private bool HealthCheckAge()
    {
        int ageCheckRoll = GameRandom.NextIntNormalized(0, 250);
        bool isAlive = ageCheckRoll > AgeInYears;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"Person died. Age: {AgeInYears}. AgeCheckRoll: {ageCheckRoll}.");
        }

        return isAlive;
    }

    protected override void TryToEat()
    {
        if (Hunger > 30) {
            FoodItem? firstFood = GameGlobals.CurrentGameState.GlobalInventory.OfType<FoodItem>().FirstOrDefault();

            if (firstFood is not null)
            {
                Hunger = Math.Max(0, Hunger - firstFood.HungerRestored + GameRandom.NextInt(2));
                GameGlobals.CurrentGameState.GlobalInventory.Remove(firstFood);
            }
        }
    }
}
