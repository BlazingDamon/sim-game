using Main.Components;
using Main.CoreGame.Base;
using Main.Entities.Materials;

namespace Main.Entities;
internal static class EntityGen
{
    public static Entity Person(int ageInYears) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Health { AgeInSeconds = (long)GameConstants.SECONDS_IN_YEAR * ageInYears + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR) },
        new Hunger(),
        new Job());

    public static Entity BuildingMaterialItem(MaterialType materialType) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Item(),
        new BuildingMaterial(materialType));

    public static Entity FoodItem(int hungerRestored) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Item(),
        new Consumable(hungerRestored));

    public static Entity NamedItem(string itemName) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Item { Name = itemName });
}
