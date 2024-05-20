using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class FoodForageJob : Job
{
    public FoodForageJob(PersonEntity assignedPerson) : base(assignedPerson)
    {
    }
}
