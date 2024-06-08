using Main.Items;
using Main.CoreGame.Base;
using Main.Entities;
using Main.Components;

namespace Main.Systems.TravelerSystems;
internal class TravelerSystem : GameSystem
{
    public TravelerSystem() : base(typeof(Health)) { }

    public override void RunSimulationFrame()
    {
        if (ISimulated.IsWeekPassedSinceLastFrame(timeOfDayInSeconds: GameConstants.SECONDS_IN_DAY / 2, dayOfWeek: 0))
        {
            int population = GetComponents<Health>().Count(x => x.Get<Health>().IsAlive);
            if (population == 0)
                return;

            int foodCount = ItemSearcher.GetEntityCount<Consumable>(x => x.Get<Consumable>().IsConsumed == false && x.Get<Consumable>().HungerRestored > 0);
            int foodStockpileRatio = foodCount / population;
            if (foodStockpileRatio > 8)
            {
                int numberOfTravelers = GameRandom.NextInt(1, 4);

                Helpers.RunMethodManyTimes(AddTravelerToPopulation, numberOfTravelers);

                if (numberOfTravelers == 1)
                {
                    GameDebugLogger.WriteLog("1 traveler has arrived!");
                    GameGlobals.CurrentGameState.GameLogger.WriteLog("One traveler has arrived!");
                }
                else if (numberOfTravelers > 1)
                {
                    GameDebugLogger.WriteLog($"{numberOfTravelers} travelers have arrived!");
                    GameGlobals.CurrentGameState.GameLogger.WriteLog($"{numberOfTravelers} travelers have arrived!");
                }
            }
        }
    }

    private void AddTravelerToPopulation()
    {
        EntityGen.Person(GameRandom.NextInt(25, 65));
    }
}
