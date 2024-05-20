using Main.Entities.Base;
using Main.Items.Material;

namespace Main.Entities.Buildings;
internal class LumberMillBuilding : Building
{
    public override void RunSimulationFrame()
    {
        if (AssignedJob is not null)
        {
            FramesSinceLastProduct++;

            if (FramesSinceLastProduct * GameConfig.TimePerFrameInSeconds > SecondsToProduceProduct)
            {
                int howManyProducts = GameRandom.NextInt(3, 5);
                Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.GlobalInventory.Add(new WoodItem()), howManyProducts);

                FramesSinceLastProduct = 0;
            }
        }
    }
}
