using Main.Components.Enums;
using Main.CoreGame.Base;

namespace Main.Components;
internal class Employment : IGameComponent
{
    public bool IsEmployed { get; set; }
    public JobType JobType { get; set; }
    public ulong? BuildingId { get; set; }
    public BuildingType? BuildingType { get; set; }
    public string? PlainName { get; set; }

    public static void Unassign(Employment employment)
    {
        if (employment.BuildingId is not null)
        {
            Building? building = GameGlobals.CurrentGameState.Components.GetGameComponent<Building>(employment.BuildingId.Value);
            if (building is not null)
            {
                building.AssignedEmployeeId = null;
            }
        }

        employment.IsEmployed = false;
        employment.JobType = JobType.Unspecified;
        employment.BuildingId = null;
        employment.BuildingType = null;
        employment.PlainName = null;
    }

    public static void Assign(Employment employment, ulong buildingId, BuildingType buildingType, string plainName)
    {
        employment.IsEmployed = true;
        employment.JobType = JobType.Unspecified;
        employment.BuildingId = buildingId;
        employment.BuildingType = buildingType;
        employment.PlainName = plainName;
    }

    public static void Assign(Employment employment, JobType jobType, string plainName)
    {
        employment.IsEmployed = true;
        employment.JobType = jobType;
        employment.PlainName = plainName;
    }
}
