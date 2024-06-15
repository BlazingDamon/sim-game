using Main.Components.Enums;
using Main.CoreGame.Base;
using Main.Systems.JobSystems.Base;

namespace Main.Components;
internal class Building : IGameComponent
{
    public BuildingType BuildingType { get; init; }
    public BaseJob? AssignedJob { get; set; }
    public int FramesSinceLastProduct { get; set; }

    public int SecondsToProduceProduct { get; set; } = GameConstants.SECONDS_IN_DAY * 2;

    public string RecommendedJobPlainName { get; set; } = "working at a building";

    public Building(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Farm:
                BuildingType = BuildingType.Farm;
                RecommendedJobPlainName = "working on a farm";
                break;
            case BuildingType.LumberMill:
                BuildingType = BuildingType.LumberMill;
                RecommendedJobPlainName = "sawing away at a lumber mill";
                break;
            case BuildingType.Quarry:
                BuildingType = BuildingType.Quarry;
                RecommendedJobPlainName = "digging up stone at a quarry";
                break;
            case BuildingType.StatueWorkshop:
                BuildingType = BuildingType.StatueWorkshop;
                RecommendedJobPlainName = "carefully crafting at a statue workshop";
                SecondsToProduceProduct = GameConstants.SECONDS_IN_DAY * 3;
                break;
            case BuildingType.Unknown:
                break;
        }
    }
}
