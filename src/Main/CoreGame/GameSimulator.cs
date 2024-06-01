using Main.CoreGame;
using Main.DebugUtils;
using Main.DebugUtils.Models;
using System.Diagnostics;

namespace Main;

internal class GameSimulator
{
    private static Stopwatch sw = new();

    public static void RunFrame()
    {
        GameGlobals.CurrentGameState.FramesPassed++;

        sw.Restart();
        foreach (var system in GameGlobals.CurrentGameState.Systems2.GetGameSystems())
        {
            system.RunSimulationFrame();
        }

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

        sw.Stop();

        GameDebugStats.WriteFrameTimeStats(
            new FrameTimeStats
            {
                SimulationTimeInNanoseconds = (long)(sw.Elapsed.TotalMilliseconds * 1_000_000)
            });
    }
}
