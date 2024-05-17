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
                new PersonEntity { AgeInSeconds = 86400000L * 13 },
                new PersonEntity { AgeInSeconds = 86400000L * 19 },
                new PersonEntity { AgeInSeconds = 86400000L * 25 },
            });
    }
}
