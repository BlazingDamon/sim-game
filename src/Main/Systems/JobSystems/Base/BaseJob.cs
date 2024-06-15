using Main.Components;

namespace Main.Systems.JobSystems.Base;
internal class BaseJob
{
    public ulong AssignedWorkerId { get; set; }
    public Building? Building { get; set; }
    public string PlainName { get; init; }

    public BaseJob(string plainName, ulong assignedWorkerId, Building? building = default)
    {
        PlainName = plainName;
        AssignedWorkerId = assignedWorkerId;
        Building = building;
    }

    public void Unassign(Job assignedJob)
    {
        assignedJob.CurrentJob = null;
        
        if (Building?.AssignedJob is not null)
            Building.AssignedJob = null;
    }
}
