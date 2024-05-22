using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class FoodForageJob : Job
{
    private static readonly string _plainName = "foraging for food";
    public FoodForageJob(PersonEntity assignedPerson) : base(_plainName, assignedPerson)
    {
    }
}
