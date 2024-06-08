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

        foreach (var system in GameGlobals.CurrentGameState.Systems.GetGameSystems())
        {
            system.RunSimulationFrame();
        }

        GameGlobals.CurrentGameState.Entities.FinalizeFrame();
        GameGlobals.CurrentGameState.Components.FinalizeFrame();

        sw.Stop();

        GameDebugStats.WriteFrameTimeStats(
            new FrameTimeStats
            {
                SimulationTimeInNanoseconds = (long)(sw.Elapsed.TotalMilliseconds * 1_000_000)
            });
    }
}
