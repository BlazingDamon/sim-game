using Main.Items.Food.Base;
using Main.Items;

namespace Main.Systems.TravelerSystems;
internal class TravelerSystem : ISimulated
{
    public void RunSimulationFrame()
    {
        if (ISimulated.IsWeekPassedSinceLastFrame(timeOfDayInSeconds: GameConstants.SECONDS_IN_DAY / 2, dayOfWeek: 0))
        {
            int population = GameGlobals.CurrentGameState.SimulatedEntities.OfType<PersonEntity>().Count(x => x.IsAlive);
            if (population == 0)
                return;

            int foodCount = ItemSearcher.GetItemCount<FoodItem>();
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
        GameGlobals.CurrentGameState.SimulatedEntities.Add(
            new PersonEntity
            {
                AgeInSeconds = GameConstants.SECONDS_IN_YEAR * GameRandom.NextInt(25, 65) + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR)
            });
    }
}
