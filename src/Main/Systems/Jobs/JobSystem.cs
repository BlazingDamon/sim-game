using Main.Entities.Base;
using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class JobSystem : ISimulated
{
    public void RunSimulationFrame()
    {
        if (ISimulated.IsDayPassedSinceLastFrame(GameConstants.SECONDS_IN_DAY / 4))
        {
            List<PersonEntity> allUnassignedPeople = GameGlobals.CurrentGameState.SimulatedEntities
                .OfType<PersonEntity>().Where(x => x.IsAlive && (x.CurrentJob is null || x.CurrentJob is MaterialsForageJob)).ToList();
            IEnumerable<Building> allUnassignedBuildings = GameGlobals.CurrentGameState.Buildings.OfType<Building>().Where(x => x.AssignedJob is null);

            foreach (var building in allUnassignedBuildings)
            {
                PersonEntity? firstAvailablePerson = allUnassignedPeople.FirstOrDefault();
                if (firstAvailablePerson is not null)
                {
                    if (firstAvailablePerson.CurrentJob is not null)
                        firstAvailablePerson.CurrentJob.Unassign();

                    var job = new BaseJob(building.RecommendedJobPlainName, firstAvailablePerson, building);
                    firstAvailablePerson.CurrentJob = job;
                    building.AssignedJob = job;
                    allUnassignedPeople.Remove(firstAvailablePerson);
                }
                else
                {
                    break;
                }
            }

            foreach (var unassignedPerson in allUnassignedPeople)
            {
                var job = new MaterialsForageJob(unassignedPerson);
                unassignedPerson.CurrentJob = job;
            }
        }
    }
}
