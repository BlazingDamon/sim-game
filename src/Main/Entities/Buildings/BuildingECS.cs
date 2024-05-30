using Main.CoreGame.Base;
using Main.Systems.JobSystems.Base;

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
                RecommendedJobPlainName = "working on a farm";
                break;
            case BuildingType.Unknown:
                break;
        }
    }
}
