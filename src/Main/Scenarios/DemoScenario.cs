using Main.Entities;
using Main.Entities.Materials;
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
        GameGlobals.CurrentGameState.Systems2.Register(new EventSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new HealthSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new HungerSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new JobSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new ForageSystem());
        GameGlobals.CurrentGameState.Systems2.Register(new TravelerSystem());
        EntityGen.Person(42);
        EntityGen.Person(33);
        EntityGen.Person(50);
        // load tester helper:
        //Helpers.RunMethodManyTimes(() => EntityGen.Person(20 + GameRandom.NextInt(60)), 1000);

        Helpers.RunMethodManyTimes(() => EntityGen.FoodItem(25), 50);
        Helpers.RunMethodManyTimes(() => EntityGen.BuildingMaterialItem(MaterialType.Wood), 10);
        Helpers.RunMethodManyTimes(() => EntityGen.BuildingMaterialItem(MaterialType.Stone), 5);

        GameGlobals.MenuStack.Push(new BuildingMenu());
    }
}
