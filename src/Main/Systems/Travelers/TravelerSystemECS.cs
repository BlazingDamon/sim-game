using Main.Items.Food.Base;
using Main.Items;
using Main.CoreGame.Base;
using Main.CoreGame;
using Main.Components;

namespace Main.Systems.Travelers;
internal class TravelerSystemECS : GameSystem
{
    public TravelerSystemECS() : base(typeof(Components.Health)) { }

    public override void RunSimulationFrame()
    {
        if (ISimulated.IsWeekPassedSinceLastFrame(timeOfDayInSeconds: GameConstants.SECONDS_IN_DAY / 2, dayOfWeek: 0))
        {
            int population = _componentDictionary[typeof(Components.Health)].Count(x => ((Components.Health)x.Component).IsAlive);
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
        var p1 = GameManager.CreateEntity();
        GameGlobals.CurrentGameState.Components.Register(p1.Id, new Components.Health { AgeInSeconds = GameConstants.SECONDS_IN_YEAR * GameRandom.NextInt(25, 65) + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR) });
        GameGlobals.CurrentGameState.Components.Register(p1.Id, new Components.Hunger());
        GameGlobals.CurrentGameState.Components.Register(p1.Id, new Job());
    }
}
