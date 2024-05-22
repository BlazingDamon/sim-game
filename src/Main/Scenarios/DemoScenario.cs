using Main.Items.Food;
using Main.Items.Material;
using Main.Menus;
using Main.Systems.Events;
using Main.Systems.Jobs;
using Main.Systems.Travelers;

namespace Main;
internal class DemoScenario : IScenario
{
    public void Initialize()
    {
        InitializePeopleList();
    }

    private static void InitializePeopleList()
    {
        GameGlobals.CurrentGameState.Systems.AddRange(
            new List<ISimulated>
            {
                new EmergencyJobSystem(),
                new JobSystem(),
                new TravelerSystem(),
                new EventSystem(),
            });

        GameGlobals.CurrentGameState.SimulatedEntities.AddRange(
            new List<ISimulated>
            {
                new PersonEntity { AgeInSeconds = 86400000L * 13 + GameRandom.NextInt(GameConstants.SECONDS_IN_MONTH)},
                new PersonEntity { AgeInSeconds = 86400000L * 19 + GameRandom.NextInt(GameConstants.SECONDS_IN_MONTH)},
                new PersonEntity { AgeInSeconds = 86400000L * 25 + GameRandom.NextInt(GameConstants.SECONDS_IN_MONTH)},
            });

        var itemList = new List<Item>();
        Helpers.RunMethodManyTimes(() => itemList.Add(new FarmedFoodItem()), 50);
        Helpers.RunMethodManyTimes(() => itemList.Add(new WoodItem()), 10);
        Helpers.RunMethodManyTimes(() => itemList.Add(new StoneItem()), 5);
        GameGlobals.CurrentGameState.GlobalInventory.AddRange(itemList);

        GameGlobals.MenuStack.Push(new BuildingMenu());
    }
}
