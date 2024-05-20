using Main.Entities.Buildings;
using Main.Items;
using Main.Items.Food.Base;
using Main.Items.Material;

namespace Main;

internal class GameBaker
{
    private static string[] _summaryView = [];

    public static string[] BakedSummaryView => _summaryView;

    public static void BakeAll()
    {
        _summaryView = BakeSummaryView();
    }

    private static string[] BakeSummaryView()
    {
        List<string> stringList = new();
        stringList.Add($"Years Passed: {(GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_YEAR}");
        stringList.Add($"Days Passed: {(GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_DAY}");
        stringList.Add("");
        stringList.Add($"Food Storage: {ItemSearcher.GetItemCount<FoodItem>()}");
        stringList.Add($"Wood: {ItemSearcher.GetItemCount<WoodItem>()}, Stone: {ItemSearcher.GetItemCount<StoneItem>()}");
        stringList.Add($"Farms : {GameGlobals.CurrentGameState.Buildings.Count(x => x is FarmBuilding)}" +
                       $", Lumber Mills: {GameGlobals.CurrentGameState.Buildings.Count(x => x is LumberMillBuilding)}" +
                       $", Quarries: {GameGlobals.CurrentGameState.Buildings.Count(x => x is QuarryBuilding)}");
        stringList.Add("");
        foreach (var simEntity in GameGlobals.CurrentGameState.SimulatedEntities)
        {
            if (simEntity is PersonEntity person)
                stringList.Add($"Person Age: {(person.AgeInSeconds / GameConstants.SECONDS_IN_YEAR):0} years. " +
                    $"Health: {person.Health}. Hunger: {person.Hunger}. " +
                    $"Job: {(person.CurrentJob is null ? "No" : "Yes")}. " +
                    $"Status: {(person.IsAlive ? "Alive" : "Dead...")}");
        }
        return stringList.ToArray();
    }
}
