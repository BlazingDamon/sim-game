using Main.Components;
using Main.CoreGame.Base;
using Main.Items;
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
                    .Where(x => x.Get<Building>().AssignedEmployeeId is null)
                    .Where(x => x.Get<Building>().BuildingType == BuildingType.Farm)
                    .ToList();

                foreach (var employmentComponent in GetComponents<Employment>())
                {
                    Employment employment = employmentComponent.Get<Employment>();
                    if (!employment.IsEmployed || employment.BuildingType != BuildingType.Farm)
                    {
                        if (employment.IsEmployed)
                            Employment.Unassign(employment);

                        var firstUnassignedFarm = allUnassignedFarms.FirstOrDefault();
                        if (firstUnassignedFarm is not null)
                        {
                            Building unassignedFarm = firstUnassignedFarm.Get<Building>();
                            Employment.Assign(employment, firstUnassignedFarm.EntityId, unassignedFarm.BuildingType, unassignedFarm.RecommendedJobPlainName);
                            unassignedFarm.AssignedEmployeeId = employmentComponent.EntityId;
                            allUnassignedFarms.Remove(firstUnassignedFarm);
                        }
                        else
                        {
                            Employment.Assign(employment, JobType.FoodForage, "foraging for food");
                        }
                    }

                }
            }
            else
            {
                foreach (var employmentEntity in GetComponents<Employment>())
                {
                    Employment employment = employmentEntity.Get<Employment>();
                    if (employment.IsEmployed && employment.JobType is JobType.FoodForage)
                    {
                        Employment.Unassign(employment);
                    }
                }
            }

            List<EntityComponent> allUnassignedWorkers = GetComponents<Employment>().Where(x => !x.Get<Employment>().IsEmployed || x.Get<Employment>().JobType is JobType.MaterialsForage).ToList();
            List<EntityComponent> allUnassignedBuildings = GetComponents<Building>().Where(x => x.Get<Building>().AssignedEmployeeId is null).ToList();

            foreach (var building in allUnassignedBuildings)
            {
                EntityComponent? firstAvailableWorker = allUnassignedWorkers.FirstOrDefault();
                if (firstAvailableWorker is not null)
                {
                    ulong firstWorkerId = firstAvailableWorker.EntityId;
                    Employment firstWorkerEmployment = firstAvailableWorker.Get<Employment>();
                    if (firstWorkerEmployment.IsEmployed)
                        Employment.Unassign(firstWorkerEmployment);

                    var unassignedBuilding = building.Get<Building>();
                    Employment.Assign(firstWorkerEmployment, building.EntityId, unassignedBuilding.BuildingType, unassignedBuilding.RecommendedJobPlainName);
                    unassignedBuilding.AssignedEmployeeId = firstAvailableWorker.EntityId;
                    allUnassignedWorkers.Remove(firstAvailableWorker);
                }
                else
                {
                    break;
                }
            }

            foreach (var unassignedWorker in allUnassignedWorkers)
            {
                Employment.Assign(unassignedWorker.Get<Employment>(), JobType.MaterialsForage, "foraging for wood and stone");
            }
        }

        IEnumerable<EntityComponent> allBuildingsWithWorkers = GetComponents<Building>()
                    .Where(x => x.Get<Building>().AssignedEmployeeId is not null);

        foreach (EntityComponent buildingWithWorker in allBuildingsWithWorkers)
        {
            Building building = buildingWithWorker.Get<Building>();
            if (building.AssignedEmployeeId is not null)
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
