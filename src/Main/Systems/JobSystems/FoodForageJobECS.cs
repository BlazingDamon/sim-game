using Main.Systems.JobSystems.Base;

namespace Main.Systems.JobSystems;
internal class FoodForageJobECS : BaseJobECS
{
    private static readonly string _plainName = "foraging for food";
    public FoodForageJobECS(ulong assignedWorkerId) : base(_plainName, assignedWorkerId)
    {
    }
}
