using Main.Components;
using Main.Components.Enums;
using Main.CoreGame.Base;

namespace Main.Entities;
internal static class EntityGen
{
    public static Entity Person(int ageInYears) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Health { AgeInSeconds = (long)GameConstants.SECONDS_IN_YEAR * ageInYears + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR) },
        new Hunger(),
        new Employment());

    public static Entity BuildingMaterialItem(MaterialType materialType) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Item(),
        new BuildingMaterial(materialType));

    public static Entity FoodItem(int hungerRestored) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Item(),
        new Consumable(hungerRestored));

    public static Entity NamedItem(string itemName) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
        new Item { Name = itemName });
}
