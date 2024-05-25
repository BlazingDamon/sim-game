using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class FoodForageJobECS : BaseJobECS
{
    private static readonly string _plainName = "foraging for food";
    public FoodForageJobECS(ulong assignedWorkerId) : base(_plainName, assignedWorkerId)
    {
    }
}
