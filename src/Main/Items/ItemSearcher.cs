using Main.Components;
using Main.Components.Enums;
using Main.CoreGame.Base;

namespace Main.Items;
internal static class ItemSearcher
{
    public static int GetEntityCount<T>() where T : IGameComponent =>
        GameGlobals.CurrentGameState.Components.GetEntityComponents<T>().Count;

    public static int GetEntityCount<T>(Func<EntityComponent, bool> predicate) where T : IGameComponent =>
        GameGlobals.CurrentGameState.Components.GetEntityComponents<T>(predicate).Count;

    public static int GetItemCountByName(string name) =>
        GameGlobals.CurrentGameState.Components.GetEntityComponents<Item>()
            .Count(x => x.Get<Item>().Name == name);

    public static int GetBuildingMaterialCountByMaterialType(MaterialType materialType) =>
        GameGlobals.CurrentGameState.Components.GetEntityComponents<BuildingMaterial>()
            .Count(x => x.Get<BuildingMaterial>().MaterialType == materialType);

    public static bool CheckBuildingMaterialCountIsAtLeast(MaterialType materialType, int checkCount)
    {
        var currentCount = 0;
        GameGlobals.CurrentGameState.Components.GetEntityComponents<BuildingMaterial>()
            .Where(x => x.Get<BuildingMaterial>().MaterialType == materialType)
            .TakeWhile(x =>
            {
                currentCount += 1;

                return currentCount < checkCount;
            }).ToList();

        return currentCount >= checkCount;
    }

    public static bool TryUseBuildingMaterial(MaterialType materialType, int count = 1)
    {
        if (!CheckBuildingMaterialCountIsAtLeast(materialType, count))
            return false;

        int usedItems = 0;
        List<EntityComponent> buildingMaterials = GameGlobals.CurrentGameState.Components
            .GetEntityComponents<BuildingMaterial>()
            .Where(x => x.Get<BuildingMaterial>().MaterialType == materialType)
            .ToList();
        while (usedItems < count && usedItems < buildingMaterials.Count)
        {
            EntityComponent foundMaterial = buildingMaterials[usedItems];
            usedItems++;
            GameGlobals.CurrentGameState.Entities.DeleteEntity(foundMaterial.EntityId);
        }

        return true;
    }
}
