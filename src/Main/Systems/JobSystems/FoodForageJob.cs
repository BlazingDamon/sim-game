using Main.Systems.JobSystems.Base;

namespace Main.Systems.JobSystems;
internal class FoodForageJob : BaseJob
{
    private static readonly string _plainName = "foraging for food";
    public FoodForageJob(ulong assignedWorkerId) : base(_plainName, assignedWorkerId)
    {
    }
}
