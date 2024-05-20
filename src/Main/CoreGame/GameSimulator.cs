namespace Main;

internal class GameSimulator
{
    public static void RunFrame()
    {
        GameGlobals.CurrentGameState.FramesPassed++;

        foreach (var system in GameGlobals.CurrentGameState.Systems)
        {
            system.RunSimulationFrame();
        }

        foreach (var building in GameGlobals.CurrentGameState.Buildings)
        {
            building.RunSimulationFrame();
        }

        foreach (var simEntity in GameGlobals.CurrentGameState.SimulatedEntities)
        {
            simEntity.RunSimulationFrame();
        }
    }
}
