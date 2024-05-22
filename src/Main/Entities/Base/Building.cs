using Main.Systems.Jobs.Base;

namespace Main.Entities.Base;
internal abstract class Building : SimulatedEntity
{
    public Job? AssignedJob { get; set; }
    public int FramesSinceLastProduct { get; set; }

    public int SecondsToProduceProduct { get; set; } = GameConstants.SECONDS_IN_DAY * 2;
    public string RecommendedJobPlainName { get; set; } = "working at a building";
}
