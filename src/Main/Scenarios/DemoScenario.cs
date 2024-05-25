using Main.Components;
using Main.CoreGame;
using Main.CoreGame.Base;
using Main.Items.Food;
using Main.Items.Material;
using Main.Menus;
using Main.Systems.Events;
using Main.Systems.Health;
using Main.Systems.Hunger;
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
        #region ECS scenario

        GameGlobals.CurrentGameState.Systems2.Register(new EventSystemECS());
        GameGlobals.CurrentGameState.Systems2.Register(new HealthSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new HungerSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new JobSystemECS());
        GameGlobals.CurrentGameState.Systems2.Register(new TravelerSystemECS());
        var p1 = GameManager.CreateEntity();
        var p2 = GameManager.CreateEntity();
        GameGlobals.CurrentGameState.Components.Register(p1.Id, new Health { AgeInSeconds = GameConstants.SECONDS_IN_YEAR * 42 + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR) });
        GameGlobals.CurrentGameState.Components.Register(p1.Id, new Hunger());
        GameGlobals.CurrentGameState.Components.Register(p1.Id, new Job());
        GameGlobals.CurrentGameState.Components.Register(p2.Id, new Health { AgeInSeconds = GameConstants.SECONDS_IN_YEAR * 33 + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR) });
        GameGlobals.CurrentGameState.Components.Register(p2.Id, new Hunger());
        GameGlobals.CurrentGameState.Components.Register(p2.Id, new Job());

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
        Helpers.RunMethodManyTimes(() => itemList.Add(new FarmedFoodItem()), 50);
        Helpers.RunMethodManyTimes(() => itemList.Add(new WoodItem()), 10);
        Helpers.RunMethodManyTimes(() => itemList.Add(new StoneItem()), 5);
        GameGlobals.CurrentGameState.GlobalInventory.AddRange(itemList);

        GameGlobals.MenuStack.Push(new BuildingMenu());
    }
}
