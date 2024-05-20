using Main.Systems.Jobs.Base;

namespace Main.Entities.Base;
internal abstract class Building : SimulatedEntity
{
    public Job? AssignedJob { get; set; }
    public int FramesSinceLastProduct { get; set; }
    public virtual int SecondsToProduceProduct { get; set; } = GameConstants.SECONDS_IN_DAY * 1;
}
