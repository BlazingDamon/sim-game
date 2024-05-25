using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class MaterialsForageJobECS : BaseJobECS
{
    private static readonly string _plainName = "foraging for wood and stone";
    public MaterialsForageJobECS(ulong assignedWorkerId) : base(_plainName, assignedWorkerId)
    {
    }
}
