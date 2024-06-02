using Main.Entities;
using Main.Entities.Materials;
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
        GameGlobals.CurrentGameState.Systems2.Register(new ForageSystemECS());
        GameGlobals.CurrentGameState.Systems2.Register(new TravelerSystemECS());
        EntityGen.Person(42);
        EntityGen.Person(33);
        EntityGen.Person(50);
        // load tester helper:
        //Helpers.RunMethodManyTimes(() => EntityGen.Person(20 + GameRandom.NextInt(60)), 1000);

        Helpers.RunMethodManyTimes(() => EntityGen.FoodItem(25), 50);
        Helpers.RunMethodManyTimes(() => EntityGen.BuildingMaterialItem(MaterialType.Wood), 10);
        Helpers.RunMethodManyTimes(() => EntityGen.BuildingMaterialItem(MaterialType.Stone), 5);

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

        // load tester helper:
        //Helpers.RunMethodManyTimes(() => GameGlobals.CurrentGameState.SimulatedEntities.Add(new PersonEntity
        //{
        //    AgeInSeconds = (long)GameConstants.SECONDS_IN_YEAR * GameRandom.NextInt(20, 60) + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR)
        //}), 1000);

        var itemList = new List<BaseItem>();
        Helpers.RunMethodManyTimes(() => itemList.Add(new FarmedFoodItem()), 50);
        Helpers.RunMethodManyTimes(() => itemList.Add(new WoodItem()), 10);
        Helpers.RunMethodManyTimes(() => itemList.Add(new StoneItem()), 5);
        GameGlobals.CurrentGameState.GlobalInventory.AddRange(itemList);

        GameGlobals.MenuStack.Push(new BuildingMenu());
    }
}
