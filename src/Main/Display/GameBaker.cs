using Main.Components;
using Main.Components.Enums;
using Main.Items;
using Main.Systems.JobSystems;

namespace Main;

internal class GameBaker
{
    private static string[] _summaryView = [];
    private static string[] _overview = [];

    public static string[] BakedSummaryView => _summaryView;
    public static string[] BakedOverview => _overview;

    public static void BakeAll()
    {
        _summaryView = BakeSummaryView();
        _overview = BakeOverview();
    }

    private static string[] BakeSummaryView()
    {
        List<string> stringList = new();
        //stringList.Add($"Years Passed: {(GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_YEAR}");
        
        stringList.Add($"Current Storage");
        stringList.Add($"Food:    {ItemSearcher.GetEntityCount<Consumable>(x => x.Get<Consumable>().HungerRestored > 0),3}");
        stringList.Add($"Wood:    {ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Wood),3}");
        stringList.Add($"Stone:   {ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Stone),3}");
        stringList.Add($"Statues: {ItemSearcher.GetItemCountByName("Statue"),3}");
        stringList.Add("");

        int numberOfBuildings = GameGlobals.CurrentGameState.Entities.QueryByType(typeof(Building)).Count;
        stringList.Add($"Current Buildings ({numberOfBuildings})");
        stringList.Add($"Farms:            {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(Building)).Count(x => x.Get<Building>().BuildingType == BuildingType.Farm), 2}");
        stringList.Add($"Lumber Mills:     {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(Building)).Count(x => x.Get<Building>().BuildingType == BuildingType.LumberMill), 2}");
        stringList.Add($"Quarries:         {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(Building)).Count(x => x.Get<Building>().BuildingType == BuildingType.Quarry), 2}");
        stringList.Add($"Statue Workshops: {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(Building)).Count(x => x.Get<Building>().BuildingType == BuildingType.StatueWorkshop), 2}");

        stringList.Add("");

        int peoplePopulation2 = GameGlobals.CurrentGameState.Entities
            .QueryByTypes(typeof(Health), typeof(Hunger), typeof(Employment))
            .Count(x => x.Get<Health>().IsAlive);
        int peopleWithoutJobs2 = GameGlobals.CurrentGameState.Entities
            .QueryByTypes(typeof(Health), typeof(Hunger), typeof(Employment))
            .Count(x => x.Get<Health>().IsAlive &&
                    (x.Get<Employment>().CurrentJob is null ||
                    x.Get<Employment>().CurrentJob is FoodForageJob ||
                    x.Get<Employment>().CurrentJob is MaterialsForageJob));
        stringList.Add($"People Overview (Population: {peoplePopulation2}) ({(peopleWithoutJobs2 == 1 ? "1 person does not have a job" : peopleWithoutJobs2 + " people do not have a job")})");
        
        foreach (var entityWithComponents in GameGlobals.CurrentGameState.Entities.QueryByTypes(typeof(Health), typeof(Hunger), typeof(Employment)))
        {
            Health health = entityWithComponents.Get<Health>();
            Hunger hunger = entityWithComponents.Get<Hunger>();
            Employment job = entityWithComponents.Get<Employment>();
            stringList.Add($"Age: {health.AgeInYears:0} years. " +
                $"Occupation: {(job.CurrentJob is null ? "resting" : $"{job.CurrentJob.PlainName}")}. " +
                $"{(health.HealthPoints < 40 ? "Feeling sickly. " : "")}{(hunger.HungerPoints > 50 ? "Feeling very hungry. " : "")}" +
                $"{(health.IsAlive ? "" : "Passed away...")}");
        }

        //stringList.AddRange(TestDataGenerator.GetFillerStrings(100));

        return stringList.ToArray();
    }

    private static string[] BakeOverview()
    {
        List<string> stringList = new();

        stringList.Add($"Days Passed: {(GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_DAY,3}");
        stringList.Add($"Hour of Day: {((GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds) % GameConstants.SECONDS_IN_DAY) / GameConstants.SECONDS_IN_HOUR,3}");
        stringList.Add($"Food: {ItemSearcher.GetEntityCount<Consumable>(x => x.Get<Consumable>().HungerRestored > 0),3}       Wood:{ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Wood),3}");
        stringList.Add($"Stone:{ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Stone),3}    Statues:{ItemSearcher.GetItemCountByName("Statue"),3}");
        stringList.Add($"");
        stringList.Add("For help, press [h]");

        return stringList.ToArray();
    }
}
