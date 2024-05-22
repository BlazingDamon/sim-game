using Main.Items.Food.Base;
using Main.Items;
using Main.Entities.Buildings;
using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class EmergencyJobSystem : ISimulated
{
    public void RunSimulationFrame()
    {
        if (ISimulated.IsDayPassedSinceLastFrame(GameConstants.SECONDS_IN_DAY / 4))
        {
            var allPeople = GameGlobals.CurrentGameState.SimulatedEntities.OfType<PersonEntity>().Where(x => x.IsAlive);
            if (ItemSearcher.GetItemCount<FoodItem>() < 20)
            {
                List<FarmBuilding> allUnassignedFarms = GameGlobals.CurrentGameState.Buildings.OfType<FarmBuilding>().Where(x => x.AssignedJob is null).ToList();

                foreach (var person in allPeople)
                {
                    if (person.CurrentJob == null || person.CurrentJob.Building is not FarmBuilding)
                    {
                        if (person.CurrentJob is not null)
                            person.CurrentJob.Unassign();

                        var firstUnassignedFarm = allUnassignedFarms.FirstOrDefault();
                        if (firstUnassignedFarm is not null)
                        {
                            person.CurrentJob = new Job(firstUnassignedFarm.RecommendedJobPlainName, person, firstUnassignedFarm);
                            allUnassignedFarms.Remove(firstUnassignedFarm);
                        }
                        else
                        {
                            person.CurrentJob = new FoodForageJob(person);
                        }
                    }
                }
            }
            else
            {
                foreach (var person in allPeople)
                {
                    if (person.CurrentJob is FoodForageJob)
                    {
                        person.CurrentJob.Unassign();
                    }
                }
            }
        }
    }
}
