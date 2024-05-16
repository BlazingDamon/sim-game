namespace Main;

internal class GameSimulator
{
    public static void RunFrame()
    {
        GameGlobals.CurrentGameState.FramesPassed++;

        foreach (var simEntity in GameGlobals.CurrentGameState.SimulatedEntities)
        {
            simEntity.RunSimulationFrame();
        }
    }
}
