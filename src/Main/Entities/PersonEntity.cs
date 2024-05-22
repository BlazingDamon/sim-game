using Main.Items.Food;
using Main.Items.Food.Base;
using Main.Items.Material;
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

                if (CurrentJob is FoodForageJob && GameRandom.NextInt(3) > 1)
                {
                    GameGlobals.CurrentGameState.GlobalInventory.Add(new FarmedFoodItem());
                }
                else if (CurrentJob is MaterialsForageJob)
                {
                    int random = GameRandom.NextInt(100);
                    if (random > 75)
                    {
                        GameGlobals.CurrentGameState.GlobalInventory.Add(new StoneItem());
                    }
                    else if (random > 25)
                    {
                        GameGlobals.CurrentGameState.GlobalInventory.Add(new WoodItem());
                    }
                }
            }

            if (IsEntityAgeMonthPassedSinceLastFrame())
            {
                IsAlive = HealthCheckAge();
            }
        }
    }

    private void DoDeath()
    {
        CurrentJob?.Unassign();
    }

    private bool HealthCheck()
    {
        int healthCheckRoll = GameRandom.NextInt(30);
        bool isAlive = healthCheckRoll < Health && Health > 0;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"Person died. Health: {Health}. HealthCheckRoll: {healthCheckRoll}.");
            GameGlobals.CurrentGameState.GameLogger.WriteLog($"A person has died in poor health, at age {AgeInYears}.");
            DoDeath();
        }

        return isAlive;
    }

    private bool HealthCheckAge()
    {
        int ageCheckRoll = GameRandom.NextIntNormalized(0, 250);
        bool isAlive = ageCheckRoll > AgeInYears;

        if (!isAlive)
        {
            GameDebugLogger.WriteLog($"A person has died of natural causes, at age {AgeInYears}. AgeCheckRoll: {ageCheckRoll}.");
            GameGlobals.CurrentGameState.GameLogger.WriteLog($"A person has died of natural causes, at age {AgeInYears}.");
            DoDeath();
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
