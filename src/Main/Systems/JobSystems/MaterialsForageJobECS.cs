using Main.Systems.JobSystems.Base;

namespace Main.Systems.JobSystems;
internal class MaterialsForageJobECS : BaseJobECS
{
    private static readonly string _plainName = "foraging for wood and stone";
    public MaterialsForageJobECS(ulong assignedWorkerId) : base(_plainName, assignedWorkerId)
    {
    }
}
