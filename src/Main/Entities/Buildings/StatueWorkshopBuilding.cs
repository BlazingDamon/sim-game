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
        SecondsToProduceProduct = GameConstants.SECONDS_IN_DAY * 3;
    }

    public override void RunSimulationFrame()
    {
        if (AssignedJob is not null)
        {
            FramesSinceLastProduct++;

            if (FramesSinceLastProduct * GameConfig.TimePerFrameInSeconds > SecondsToProduceProduct)
            {
                if (ItemSearcherOld.CheckItemCountIsAtLeast<WoodItem>(40) && ItemSearcherOld.CheckItemCountIsAtLeast<StoneItem>(40))
                {
                    ItemSearcherOld.TryUseItem<WoodItem>(10);
                    ItemSearcherOld.TryUseItem<StoneItem>(10);

                    GameGlobals.CurrentGameState.GlobalInventory.Add(new StatueItem());
                    FramesSinceLastProduct = 0;
                }
            }
        }
    }
}
