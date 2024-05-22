using Main.Entities.Base;
using Main.Items;
using Main.Items.Decorative;
using Main.Items.Material;

namespace Main.Entities.Buildings;
internal class StatueWorkshopBuilding : Building
{
    public StatueWorkshopBuilding()
    {
        RecommendedJobPlainName = "carefully crafting at a statue workshop";
    }

    public override void RunSimulationFrame()
    {
        if (AssignedJob is not null)
        {
            FramesSinceLastProduct++;

            if (FramesSinceLastProduct * GameConfig.TimePerFrameInSeconds > SecondsToProduceProduct)
            {
                if (ItemSearcher.CheckItemCountIsAtLeast<WoodItem>(40) && ItemSearcher.CheckItemCountIsAtLeast<StoneItem>(40))
                {
                    ItemSearcher.TryUseItem<WoodItem>(10);
                    ItemSearcher.TryUseItem<StoneItem>(10);

                    GameGlobals.CurrentGameState.GlobalInventory.Add(new StatueItem());
                    FramesSinceLastProduct = 0;
                }
            }
        }
    }
}
