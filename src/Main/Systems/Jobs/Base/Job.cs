using Main.Entities.Base;

namespace Main.Systems.Jobs.Base;
internal class Job
{
    public PersonEntity AssignedPerson { get; set; }
    public Building? Building { get; set; }

    public Job(PersonEntity assignedPerson, Building? building = default)
    {
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
