using Main.Components;
using Main.CoreGame.Base;
using Main.Entities.Buildings;
using Main.Items.Food.Base;
using Main.Items;
using Main.Systems.JobSystems.Base;
using Main.Items.Food;
using Main.Items.Material;
using Main.Items.Decorative;

namespace Main.Systems.JobSystems;
internal class JobSystemECS : GameSystem
{
    public JobSystemECS() : base(typeof(Job), typeof(BuildingECS)) { }

    public override void RunSimulationFrame()
    {
        if (ISimulated.IsDayPassedSinceLastFrame(GameConstants.SECONDS_IN_DAY / 4))
        {
            if (ItemSearcher.GetItemCount<FoodItem>() < 20)
            {
                List<EntityComponent> allUnassignedFarms = GetComponents<BuildingECS>()
                    .Where(x => x.Get<BuildingECS>().AssignedJob is null)
                    .Where(x => x.Get<BuildingECS>().BuildingType == BuildingType.Farm)
                    .ToList();

                foreach (var jobComponent in GetComponents<Job>())
                {
                    Job job = jobComponent.Get<Job>();
                    if (job.CurrentJob == null || job.CurrentJob.Building?.BuildingType != BuildingType.Farm)
                    {
                        if (job.CurrentJob is not null)
                            job.CurrentJob.Unassign(job);

                        var firstUnassignedFarm = allUnassignedFarms.FirstOrDefault();
                        if (firstUnassignedFarm is not null)
                        {
                            var unassignedFarm = firstUnassignedFarm.Get<BuildingECS>();
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
                foreach (var jobEntity in GetComponents<Job>())
                {
                    Job job = jobEntity.Get<Job>();
                    if (job.CurrentJob is FoodForageJobECS)
                    {
                        job.CurrentJob.Unassign(job);
                    }
                }
            }

            List<EntityComponent> allUnassignedWorkers = GetComponents<Job>().Where(x => x.Get<Job>().CurrentJob is null || x.Get<Job>().CurrentJob is MaterialsForageJobECS).ToList();
            List<EntityComponent> allUnassignedBuildings = GetComponents<BuildingECS>().Where(x => x.Get<BuildingECS>().AssignedJob is null).ToList();

            foreach (var building in allUnassignedBuildings)
            {
                EntityComponent? firstAvailableWorker = allUnassignedWorkers.FirstOrDefault();
                if (firstAvailableWorker is not null)
                {
                    ulong firstWorkerId = firstAvailableWorker.EntityId;
                    Job firstWorkerJob = firstAvailableWorker.Get<Job>();
                    if (firstWorkerJob.CurrentJob is not null)
                        firstWorkerJob.CurrentJob.Unassign(firstWorkerJob);

                    var unassignedBuilding = building.Get<BuildingECS>();
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
                unassignedWorker.Get<Job>().CurrentJob = job;
            }
        }

        IEnumerable<EntityComponent> allBuildingsWithWorkers = GetComponents<BuildingECS>()
                    .Where(x => x.Get<BuildingECS>().AssignedJob is not null);

        foreach (EntityComponent buildingWithWorker in allBuildingsWithWorkers)
        {
            BuildingECS building = buildingWithWorker.Get<BuildingECS>();
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
        Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.GlobalInventory.Add(new FarmedFoodItem()), howManyProducts);

        return true;
    }

    public bool RunLumberMillFrame()
    {
        int howManyProducts = GameRandom.NextInt(3, 5);
        Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.GlobalInventory.Add(new WoodItem()), howManyProducts);

        return true;
    }

    public bool RunQuarryFrame()
    {
        int howManyProducts = GameRandom.NextInt(2, 4);
        Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.GlobalInventory.Add(new StoneItem()), howManyProducts);

        return true;
    }

    public bool RunStatueWorkshopFrame()
    {
        if (ItemSearcher.CheckItemCountIsAtLeast<WoodItem>(40) && ItemSearcher.CheckItemCountIsAtLeast<StoneItem>(40))
        {
            ItemSearcher.TryUseItem<WoodItem>(10);
            ItemSearcher.TryUseItem<StoneItem>(10);
            GameGlobals.CurrentGameState.GlobalInventory.Add(new StatueItem());

            return true;
        }

        return false;
    }
}
