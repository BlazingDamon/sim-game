using Main.Entities.Base;
using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class JobSystem : ISimulated
{
    public void RunSimulationFrame()
    {
        if (ISimulated.IsDayPassedSinceLastFrame(GameConstants.SECONDS_IN_DAY / 4))
        {
            List<PersonEntity> allUnassignedPeople = GameGlobals.CurrentGameState.SimulatedEntities.OfType<PersonEntity>().Where(x => x.CurrentJob is null).ToList();
            IEnumerable<Building> allUnassignedBuildings = GameGlobals.CurrentGameState.Buildings.OfType<Building>().Where(x => x.AssignedJob is null);

            foreach (var building in allUnassignedBuildings)
            {
                PersonEntity? firstAvailablePerson = allUnassignedPeople.FirstOrDefault();
                if (firstAvailablePerson is not null)
                {
                    var job = new Job(firstAvailablePerson, building);
                    firstAvailablePerson.CurrentJob = job;
                    building.AssignedJob = job;
                    allUnassignedPeople.Remove(firstAvailablePerson);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
