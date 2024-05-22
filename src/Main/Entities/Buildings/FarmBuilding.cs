using Main.Entities.Base;
using Main.Items.Food;

namespace Main.Entities.Buildings;
internal class FarmBuilding : Building
{
    public FarmBuilding()
    {
        RecommendedJobPlainName = "working on a farm";
    }

    public override void RunSimulationFrame()
    {
        if (AssignedJob is not null)
        {
            FramesSinceLastProduct++;

            if (FramesSinceLastProduct * GameConfig.TimePerFrameInSeconds > SecondsToProduceProduct)
            {
                int howManyProducts = GameRandom.NextInt(2, 5);
                Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.GlobalInventory.Add(new FarmedFoodItem()), howManyProducts);

                FramesSinceLastProduct = 0;
            }
        }
    }
}
