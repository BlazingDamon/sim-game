using Main.Entities;
using Main.Items.Food;
using Main.Items.Material;
using Main.Menus;
using Main.Systems.EventSystems;
using Main.Systems.HealthSystems;
using Main.Systems.HungerSystems;
using Main.Systems.JobSystems;
using Main.Systems.TravelerSystems;

namespace Main;
internal class DemoScenario : IScenario
{
    public void Initialize()
    {
        InitializePeopleList();
    }

    private static void InitializePeopleList()
    {
        #region ECS scenario

        GameGlobals.CurrentGameState.Systems2.Register(new EventSystemECS());
        GameGlobals.CurrentGameState.Systems2.Register(new HealthSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new HungerSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new JobSystemECS());
        GameGlobals.CurrentGameState.Systems2.Register(new TravelerSystemECS());
        EntityGen.Person(42);
        EntityGen.Person(33);

        #endregion

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
        Helpers.RunMethodManyTimes(() => itemList.Add(new FarmedFoodItem()), 0);
        Helpers.RunMethodManyTimes(() => itemList.Add(new WoodItem()), 10);
        Helpers.RunMethodManyTimes(() => itemList.Add(new StoneItem()), 5);
        GameGlobals.CurrentGameState.GlobalInventory.AddRange(itemList);

        GameGlobals.MenuStack.Push(new BuildingMenu());
    }
}
