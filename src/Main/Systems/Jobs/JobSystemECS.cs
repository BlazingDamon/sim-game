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
                List<(ulong, IGameComponent)> allUnassignedFarms = _componentDictionary[typeof(BuildingECS)]
                    .Where(x => ((BuildingECS)x.Item2).AssignedJob is null)
                    .Where(x => ((BuildingECS)x.Item2).BuildingType == BuildingType.Farm)
                    .ToList();

                foreach (var jobPair in _componentDictionary[typeof(Job)])
                {
                    Job job = (Job)jobPair.Item2;
                    if (job.CurrentJob == null || job.CurrentJob.Building?.BuildingType != BuildingType.Farm)
                    {
                        if (job.CurrentJob is not null)
                            job.CurrentJob.Unassign();

                        var firstUnassignedFarm = allUnassignedFarms.FirstOrDefault();
                        if (firstUnassignedFarm != default(ValueTuple<ulong, IGameComponent>))
                        {
                            var unassignedFarm = firstUnassignedFarm.Item2 as BuildingECS;
                            job.CurrentJob = new BaseJobECS(unassignedFarm!.RecommendedJobPlainName, jobPair.Item1, unassignedFarm);
                            allUnassignedFarms.Remove(firstUnassignedFarm);
                        }
                        else
                        {
                            job.CurrentJob = new FoodForageJobECS(jobPair.Item1);
                        }
                    }

                }
            }
            else
            {
                foreach (var jobPair in _componentDictionary[typeof(Job)])
                {
                    Job job = (Job)jobPair.Item2;
                    if (job.CurrentJob is FoodForageJobECS)
                    {
                        job.CurrentJob.Unassign();
                    }
                }
            }

            List<(ulong, IGameComponent)> allUnassignedWorkers = _componentDictionary[typeof(Job)].Where(x => ((Job)x.Item2).CurrentJob is null || ((Job)x.Item2).CurrentJob is MaterialsForageJobECS).ToList();
            List<(ulong, IGameComponent)> allUnassignedBuildings = _componentDictionary[typeof(BuildingECS)].Where(x => ((BuildingECS)x.Item2).AssignedJob is null).ToList();

            foreach (var building in allUnassignedBuildings)
            {
                (ulong, IGameComponent) firstAvailableWorker = allUnassignedWorkers.FirstOrDefault();
                if (firstAvailableWorker != default(ValueTuple<ulong, IGameComponent>))
                {
                    ulong firstWorkerId = firstAvailableWorker.Item1;
                    Job firstWorkerJob = (Job)firstAvailableWorker.Item2;
                    if (firstWorkerJob.CurrentJob is not null)
                        firstWorkerJob.CurrentJob.Unassign();

                    var unassignedBuilding = building.Item2 as BuildingECS;
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
                var job = new MaterialsForageJobECS(unassignedWorker.Item1);
                ((Job)unassignedWorker.Item2).CurrentJob = job;
            }
        }

    }
}
