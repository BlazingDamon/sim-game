using Main.Components;
using Main.CoreGame.Base;
using Main.Entities.Buildings;
using Main.Items.Food.Base;
using Main.Items;
using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class JobSystemECS : GameSystem
{
    public JobSystemECS() : base(typeof(Job), typeof(BuildingECS)) { }

    public override void RunSimulationFrame()
    {
        if (ISimulated.IsDayPassedSinceLastFrame(GameConstants.SECONDS_IN_DAY / 4))
        {
            if (ItemSearcher.GetItemCount<FoodItem>() < 50)
            {
                List<EntityComponent> allUnassignedFarms = _componentDictionary[typeof(BuildingECS)]
                    .Where(x => ((BuildingECS)x.Component).AssignedJob is null)
                    .Where(x => ((BuildingECS)x.Component).BuildingType == BuildingType.Farm)
                    .ToList();

                foreach (var jobComponent in _componentDictionary[typeof(Job)])
                {
                    Job job = (Job)jobComponent.Component;
                    if (job.CurrentJob == null || job.CurrentJob.Building?.BuildingType != BuildingType.Farm)
                    {
                        if (job.CurrentJob is not null)
                            job.CurrentJob.Unassign();

                        var firstUnassignedFarm = allUnassignedFarms.FirstOrDefault();
                        if (firstUnassignedFarm is not null)
                        {
                            var unassignedFarm = firstUnassignedFarm.Component as BuildingECS;
                            var farmJob = new BaseJobECS(unassignedFarm!.RecommendedJobPlainName, jobComponent.EntityId, unassignedFarm);
                            job.CurrentJob = farmJob;
                            unassignedFarm.AssignedJob = farmJob;
                            allUnassignedFarms.Remove(firstUnassignedFarm);
                        }
                        else
                        {
                            job.CurrentJob = new FoodForageJobECS(jobComponent.EntityId);
                        }
                    }

                }
            }
            else
            {
                foreach (var jobPair in _componentDictionary[typeof(Job)])
                {
                    Job job = (Job)jobPair.Component;
                    if (job.CurrentJob is FoodForageJobECS)
                    {
                        job.CurrentJob.Unassign();
                    }
                }
            }

            List<EntityComponent> allUnassignedWorkers = _componentDictionary[typeof(Job)].Where(x => ((Job)x.Component).CurrentJob is null || ((Job)x.Component).CurrentJob is MaterialsForageJobECS).ToList();
            List<EntityComponent> allUnassignedBuildings = _componentDictionary[typeof(BuildingECS)].Where(x => ((BuildingECS)x.Component).AssignedJob is null).ToList();

            foreach (var building in allUnassignedBuildings)
            {
                EntityComponent firstAvailableWorker = allUnassignedWorkers.FirstOrDefault();
                if (firstAvailableWorker is not null)
                {
                    ulong firstWorkerId = firstAvailableWorker.EntityId;
                    Job firstWorkerJob = (Job)firstAvailableWorker.Component;
                    if (firstWorkerJob.CurrentJob is not null)
                        firstWorkerJob.CurrentJob.Unassign();

                    var unassignedBuilding = building.Component as BuildingECS;
                    var job = new BaseJobECS(unassignedBuilding!.RecommendedJobPlainName, firstWorkerId, unassignedBuilding);
                    firstWorkerJob.CurrentJob = job;
                    unassignedBuilding.AssignedJob = job;
                    allUnassignedWorkers.Remove(firstAvailableWorker);
                }
                else
                {
                    break;
                }
            }

            foreach (var unassignedWorker in allUnassignedWorkers)
            {
                var job = new MaterialsForageJobECS(unassignedWorker.EntityId);
                ((Job)unassignedWorker.Component).CurrentJob = job;
            }
        }

    }
}
