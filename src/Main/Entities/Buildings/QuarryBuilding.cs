using Main.Entities.Base;
using Main.Items.Material;

namespace Main.Entities.Buildings;
internal class QuarryBuilding : Building
{
    public QuarryBuilding()
    {
        RecommendedJobPlainName = "digging up stone at a quarry";
    }

    public override void RunSimulationFrame()
    {
        if (AssignedJob is not null)
        {
            FramesSinceLastProduct++;

            if (FramesSinceLastProduct * GameConfig.TimePerFrameInSeconds > SecondsToProduceProduct)
            {
                int howManyProducts = GameRandom.NextInt(2, 4);
                Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.GlobalInventory.Add(new StoneItem()), howManyProducts);

                FramesSinceLastProduct = 0;
            }
        }
    }
}
