using Main.CoreGame.Base;
using Main.Systems.Jobs.Base;

namespace Main.Entities.Buildings;
internal class BuildingECS : IGameComponent
{
    public BuildingType BuildingType { get; init; }
    public BaseJobECS? AssignedJob { get; set; }
    public int FramesSinceLastProduct { get; set; }

    public int SecondsToProduceProduct { get; set; } = GameConstants.SECONDS_IN_DAY * 2;

    public string RecommendedJobPlainName { get; set; } = "working at a building";

    public BuildingECS(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Farm:
                BuildingType = BuildingType.Farm;
                RecommendedJobPlainName = "working on a farm";
                break;
            case BuildingType.Unknown:
                break;
        }
    }

    // TODO this Farm behavior should be reflected via a new ECS system
    //public override void RunSimulationFrame()
    //{
    //    if (AssignedJob is not null)
    //    {
    //        FramesSinceLastProduct++;

    //        if (FramesSinceLastProduct * GameConfig.TimePerFrameInSeconds > SecondsToProduceProduct)
    //        {
    //            int howManyProducts = GameRandom.NextInt(2, 5);
    //            Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.GlobalInventory.Add(new FarmedFoodItem()), howManyProducts);

    //            FramesSinceLastProduct = 0;
    //        }
    //    }
    //}
}
