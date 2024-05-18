using Main.Items.Material;

namespace Main;
internal class DemoScenario : IScenario
{
    public void Initialize()
    {
        InitializePeopleList();
    }

    private static void InitializePeopleList()
    {
        GameGlobals.CurrentGameState.SimulatedEntities.AddRange(
            new List<ISimulated>
            {
                new PersonEntity { AgeInSeconds = 86400000L * 13 + GameRandom.NextInt(GameConstants.SECONDS_IN_MONTH)},
                new PersonEntity { AgeInSeconds = 86400000L * 19 + GameRandom.NextInt(GameConstants.SECONDS_IN_MONTH)},
                new PersonEntity { AgeInSeconds = 86400000L * 25 + GameRandom.NextInt(GameConstants.SECONDS_IN_MONTH)},
            });

        var itemList = new List<Item>();
        Helpers.RunMethodManyTimes(() => itemList.Add(new WoodItem()), 200);
        Helpers.RunMethodManyTimes(() => itemList.Add(new StoneItem()), 200);
        GameGlobals.CurrentGameState.GlobalInventory.AddRange(itemList);
    }
}
