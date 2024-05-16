namespace Main;

internal class GameBaker
{
    private static string[] _summaryView = [];

    public static string[] BakedSummaryView => _summaryView;

    public static void Bake()
    {
        _summaryView = BakeSummaryView();
    }

    private static string[] BakeSummaryView()
    {
        List<string> stringList = new();
        stringList.Add($"Years Passed: {(GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_YEAR}");
        stringList.Add($"Days Passed: {(GameGlobals.CurrentGameState.FramesPassed * GameConfig.TimePerFrameInSeconds) / GameConstants.SECONDS_IN_DAY}");
        stringList.Add("");
        foreach (var simEntity in GameGlobals.CurrentGameState.SimulatedEntities)
        {
            if (simEntity is PersonEntity person)
                stringList.Add($"Person Age: {(person.AgeInSeconds / GameConstants.SECONDS_IN_YEAR):0} years. Status: {(person.IsAlive ? "Alive" : "Dead...")}");
        }
        return stringList.ToArray();
    }
}
