using Main.CoreGame.Base;
using Main.Systems.JobSystems.Base;

namespace Main.Components;
internal class Employment : IGameComponent
{
    public BaseJob? CurrentJob { get; set; }

    public bool IsEmployed { get; set; }
    public ulong? BuildingId { get; set; }
    public string? PlainName { get; set; }
}
