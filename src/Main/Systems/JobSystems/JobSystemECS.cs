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
                List<EntityComponent> allUnassignedFarms = _componentDictionary[typeof(BuildingECS)]
                    .Where(x => ((BuildingECS)x.Component).AssignedJob is null)
                    .Where(x => ((BuildingECS)x.Component).BuildingType == BuildingType.Farm)
                    .ToList();

                // TODO this job component list should be filtered down based on Health.IsAlive
                // maybe denormalize IsAlive onto the Job component somehow, to remove dependency on Health component for this system?
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
                EntityComponent? firstAvailableWorker = allUnassignedWorkers.FirstOrDefault();
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

        IEnumerable<EntityComponent> allBuildingsWithWorkers = _componentDictionary[typeof(BuildingECS)]
                    .Where(x => ((BuildingECS)x.Component).AssignedJob is not null);

        foreach (EntityComponent buildingWithWorker in allBuildingsWithWorkers)
        {
            BuildingECS building = (BuildingECS)buildingWithWorker.Component;
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
