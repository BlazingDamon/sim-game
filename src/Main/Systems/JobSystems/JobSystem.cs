using Main.Components;
using Main.CoreGame.Base;
using Main.Items;
using Main.Systems.JobSystems.Base;
using Main.Entities;
using Main.Components.Enums;

namespace Main.Systems.JobSystems;
internal class JobSystem : GameSystem
{
    public JobSystem() : base(typeof(Employment), typeof(Building)) { }

    public override void RunSimulationFrame()
    {
        if (ISimulated.IsDayPassedSinceLastFrame(GameConstants.SECONDS_IN_DAY / 4))
        {
            if (ItemSearcher.GetEntityCount<Consumable>(x => x.Get<Consumable>().IsConsumed == false && x.Get<Consumable>().HungerRestored > 0) < 20)
            {
                List<EntityComponent> allUnassignedFarms = GetComponents<Building>()
                    .Where(x => x.Get<Building>().AssignedJob is null)
                    .Where(x => x.Get<Building>().BuildingType == BuildingType.Farm)
                    .ToList();

                foreach (var jobComponent in GetComponents<Employment>())
                {
                    Employment job = jobComponent.Get<Employment>();
                    if (job.CurrentJob == null || job.CurrentJob.Building?.BuildingType != BuildingType.Farm)
                    {
                        if (job.CurrentJob is not null)
                            job.CurrentJob.Unassign(job);

                        var firstUnassignedFarm = allUnassignedFarms.FirstOrDefault();
                        if (firstUnassignedFarm is not null)
                        {
                            var unassignedFarm = firstUnassignedFarm.Get<Building>();
                            var farmJob = new BaseJob(unassignedFarm!.RecommendedJobPlainName, jobComponent.EntityId, unassignedFarm);
                            job.CurrentJob = farmJob;
                            unassignedFarm.AssignedJob = farmJob;
                            allUnassignedFarms.Remove(firstUnassignedFarm);
                        }
                        else
                        {
                            job.CurrentJob = new FoodForageJob(jobComponent.EntityId);
                        }
                    }

                }
            }
            else
            {
                foreach (var jobEntity in GetComponents<Employment>())
                {
                    Employment job = jobEntity.Get<Employment>();
                    if (job.CurrentJob is FoodForageJob)
                    {
                        job.CurrentJob.Unassign(job);
                    }
                }
            }

            List<EntityComponent> allUnassignedWorkers = GetComponents<Employment>().Where(x => x.Get<Employment>().CurrentJob is null || x.Get<Employment>().CurrentJob is MaterialsForageJob).ToList();
            List<EntityComponent> allUnassignedBuildings = GetComponents<Building>().Where(x => x.Get<Building>().AssignedJob is null).ToList();

            foreach (var building in allUnassignedBuildings)
            {
                EntityComponent? firstAvailableWorker = allUnassignedWorkers.FirstOrDefault();
                if (firstAvailableWorker is not null)
                {
                    ulong firstWorkerId = firstAvailableWorker.EntityId;
                    Employment firstWorkerJob = firstAvailableWorker.Get<Employment>();
                    if (firstWorkerJob.CurrentJob is not null)
                        firstWorkerJob.CurrentJob.Unassign(firstWorkerJob);

                    var unassignedBuilding = building.Get<Building>();
                    var job = new BaseJob(unassignedBuilding!.RecommendedJobPlainName, firstWorkerId, unassignedBuilding);
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
                var job = new MaterialsForageJob(unassignedWorker.EntityId);
                unassignedWorker.Get<Employment>().CurrentJob = job;
            }
        }

        IEnumerable<EntityComponent> allBuildingsWithWorkers = GetComponents<Building>()
                    .Where(x => x.Get<Building>().AssignedJob is not null);

        foreach (EntityComponent buildingWithWorker in allBuildingsWithWorkers)
        {
            Building building = buildingWithWorker.Get<Building>();
            if (building.AssignedJob is not null)
            {
                building.FramesSinceLastProduct++;

                if (building.FramesSinceLastProduct * GameConfig.TimePerFrameInSeconds > building.SecondsToProduceProduct)
                {
                    bool productProduced = building.BuildingType switch
                    {
                        BuildingType.Farm => RunFarmFrame(),
                        BuildingType.LumberMill => RunLumberMillFrame(),
                        BuildingType.Quarry => RunQuarryFrame(),
                        BuildingType.StatueWorkshop => RunStatueWorkshopFrame(),
                        BuildingType.Unknown => false,
                        _ => throw new NotImplementedException(),
                    };

                    if (productProduced)
                        building.FramesSinceLastProduct = 0;
                }
            }
        }
    }

    public bool RunFarmFrame()
    {
        int howManyProducts = GameRandom.NextInt(2, 5);
        Helpers.RunMethodManyTimes(() => EntityGen.FoodItem(25), howManyProducts);

        return true;
    }

    public bool RunLumberMillFrame()
    {
        int howManyProducts = GameRandom.NextInt(3, 5);
        Helpers.RunMethodManyTimes(() => EntityGen.BuildingMaterialItem(MaterialType.Wood), howManyProducts);

        return true;
    }

    public bool RunQuarryFrame()
    {
        int howManyProducts = GameRandom.NextInt(2, 4);
        Helpers.RunMethodManyTimes(() => EntityGen.BuildingMaterialItem(MaterialType.Stone), howManyProducts);

        return true;
    }

    public bool RunStatueWorkshopFrame()
    {
        if (ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Wood, 40) &&
            ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Stone, 40))
        {
            ItemSearcher.TryUseBuildingMaterial(MaterialType.Wood, 10);
            ItemSearcher.TryUseBuildingMaterial(MaterialType.Stone, 10);

            EntityGen.NamedItem("Statue");

            return true;
        }

        return false;
    }
}
