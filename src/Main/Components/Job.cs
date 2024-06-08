using Main.CoreGame.Base;
using Main.Systems.JobSystems.Base;

namespace Main.Components;
internal class Job : IGameComponent
{
    public BaseJob? CurrentJob { get; set; }
}
