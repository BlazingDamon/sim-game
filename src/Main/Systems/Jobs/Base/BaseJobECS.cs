using Main.Components;
using Main.Entities.Buildings;

namespace Main.Systems.Jobs.Base;
internal class BaseJobECS
{
    public ulong AssignedWorkerId { get; set; }
    public BuildingECS? Building { get; set; }
    public string PlainName { get; init; }

    public BaseJobECS(string plainName, ulong assignedWorkerId, BuildingECS? building = default)
    {
        PlainName = plainName;
        AssignedWorkerId = assignedWorkerId;
        Building = building;
    }

    public void Unassign()
    {
        Job? assignedJob = GameGlobals.CurrentGameState.Components.GetGameComponent<Job>(AssignedWorkerId);
        if (assignedJob != null)
            assignedJob.CurrentJob = null;
        
        if (Building?.AssignedJob is not null)
            Building.AssignedJob = null;
    }
}
