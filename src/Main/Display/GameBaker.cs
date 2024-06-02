﻿using Main.Components;
using Main.Entities.Base;
using Main.Entities.Buildings;
using Main.Entities.Materials;
using Main.Items;
using Main.Items.Decorative;
using Main.Items.Food.Base;
using Main.Items.Material;
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
        stringList.Add($"Food:    {ItemSearcherOld.GetItemCount<FoodItem>(),3}");
        stringList.Add($"Wood:    {ItemSearcherOld.GetItemCount<WoodItem>(),3}");
        stringList.Add($"Stone:   {ItemSearcherOld.GetItemCount<StoneItem>(),3}");
        stringList.Add($"Statues: {ItemSearcherOld.GetItemCount<StatueItem>(),3}");
        stringList.Add("");
        int numberOfBuildings = GameGlobals.CurrentGameState.Buildings.Count(x => x is Building);
        stringList.Add($"Current Buildings ({numberOfBuildings})");
        stringList.Add($"Farms:            {GameGlobals.CurrentGameState.Buildings.Count(x => x is FarmBuilding),2}");
        stringList.Add($"Lumber Mills:     {GameGlobals.CurrentGameState.Buildings.Count(x => x is LumberMillBuilding),2}");
        stringList.Add($"Quarries:         {GameGlobals.CurrentGameState.Buildings.Count(x => x is QuarryBuilding),2}");
        stringList.Add($"Statue Workshops: {GameGlobals.CurrentGameState.Buildings.Count(x => x is StatueWorkshopBuilding),2}");

        stringList.Add("");
        int peoplePopulation = GameGlobals.CurrentGameState.SimulatedEntities
            .Count(x => x is PersonEntity person && person.IsAlive);
        int peopleWithoutJobs = GameGlobals.CurrentGameState.SimulatedEntities
            .Count(x => x is PersonEntity person &&
                person.IsAlive &&
                (person.CurrentJob is null ||
                    person.CurrentJob is FoodForageJob ||
                    person.CurrentJob is MaterialsForageJob));
        stringList.Add($"People Overview (Population: {peoplePopulation}) ({(peopleWithoutJobs == 1 ? "1 person does not have a job" : peopleWithoutJobs + " people do not have a job")})");
        foreach (var simEntity in GameGlobals.CurrentGameState.SimulatedEntities)
        {
            if (simEntity is PersonEntity person)
                stringList.Add($"Age: {(person.AgeInSeconds / GameConstants.SECONDS_IN_YEAR):0} years. " +
                    $"Occupation: {(person.CurrentJob is null ? "resting" : $"{person.CurrentJob.PlainName}")}. " +
                    $"{(person.Health < 40 ? "Feeling sickly. " : "")}{(person.Hunger > 50 ? "Feeling very hungry. " : "")}" +
                    $"{(person.IsAlive ? "" : "Passed away...")}");
        }

        stringList.Add("");

        stringList.Add($"Current Storage");
        stringList.Add($"Food:    {ItemSearcher.GetEntityCount<Consumable>(),3}");
        stringList.Add($"Wood:    {ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Wood),3}");
        stringList.Add($"Stone:   {ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Stone),3}");
        stringList.Add($"Statues: {ItemSearcher.GetItemCountByName("Statue"),3}");
        stringList.Add("");

        int numberOfBuildingsECS = GameGlobals.CurrentGameState.Entities.QueryByType(typeof(BuildingECS)).Count;
        stringList.Add($"Current Buildings ({numberOfBuildingsECS})");
        stringList.Add($"Farms:            {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(BuildingECS)).Count(x => x.Get<BuildingECS>().BuildingType == BuildingType.Farm), 2}");
        stringList.Add($"Lumber Mills:     {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(BuildingECS)).Count(x => x.Get<BuildingECS>().BuildingType == BuildingType.LumberMill), 2}");
        stringList.Add($"Quarries:         {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(BuildingECS)).Count(x => x.Get<BuildingECS>().BuildingType == BuildingType.Quarry), 2}");
        stringList.Add($"Statue Workshops: {GameGlobals.CurrentGameState.Entities.QueryByType(typeof(BuildingECS)).Count(x => x.Get<BuildingECS>().BuildingType == BuildingType.StatueWorkshop), 2}");

        stringList.Add("");

        int peoplePopulation2 = GameGlobals.CurrentGameState.Entities
            .QueryByTypes(typeof(Health), typeof(Hunger), typeof(Job))
            .Count(x => x.Get<Health>().IsAlive);
        int peopleWithoutJobs2 = GameGlobals.CurrentGameState.Entities
            .QueryByTypes(typeof(Health), typeof(Hunger), typeof(Job))
            .Count(x => x.Get<Health>().IsAlive &&
                    (x.Get<Job>().CurrentJob is null ||
                    x.Get<Job>().CurrentJob is FoodForageJobECS ||
                    x.Get<Job>().CurrentJob is MaterialsForageJobECS));
        stringList.Add($"People Overview (Population: {peoplePopulation2}) ({(peopleWithoutJobs2 == 1 ? "1 person does not have a job" : peopleWithoutJobs2 + " people do not have a job")})");
        
        foreach (var entityWithComponents in GameGlobals.CurrentGameState.Entities.QueryByTypes(typeof(Health), typeof(Hunger), typeof(Job)))
        {
            Health health = entityWithComponents.Get<Health>();
            Hunger hunger = entityWithComponents.Get<Hunger>();
            Job job = entityWithComponents.Get<Job>();
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
        stringList.Add($"Food: {ItemSearcher.GetEntityCount<Consumable>(),3}       Wood:{ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Wood),3}");
        stringList.Add($"Stone:{ItemSearcher.GetBuildingMaterialCountByMaterialType(MaterialType.Stone),3}    Statues:{ItemSearcher.GetItemCountByName("Statue"),3}");
        stringList.Add($"");
        stringList.Add("For help, press [h]");

        return stringList.ToArray();
    }
}
