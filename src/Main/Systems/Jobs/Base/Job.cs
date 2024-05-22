using Main.Entities.Base;

namespace Main.Systems.Jobs.Base;
internal class Job
{
    public PersonEntity AssignedPerson { get; set; }
    public Building? Building { get; set; }
    public string PlainName { get; init; }

    public Job(string plainName, PersonEntity assignedPerson, Building? building = default)
    {
        PlainName = plainName;
        AssignedPerson = assignedPerson;
        Building = building;
    }

    public void Unassign()
    {
        AssignedPerson.CurrentJob = null;
        
        if (Building?.AssignedJob is not null)
            Building.AssignedJob = null;
    }
}
