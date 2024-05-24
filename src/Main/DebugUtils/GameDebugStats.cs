﻿using Main.DebugUtils.Models;

namespace Main.DebugUtils;
internal class GameDebugStats
{
    private static BufferedArray<int> _nanosecondsPerSimulationFrame = new(1000);

    public static void WriteFrameTimeStats(FrameTimeStats stats)
    {
        _nanosecondsPerSimulationFrame.WriteValue(stats.SimulationTimeInNanoseconds);
    }

    public static FrameTimeStats GetAverageFrameTimeStats(int frameCount) =>
        new FrameTimeStats
        {
            SimulationTimeInNanoseconds = (int)_nanosecondsPerSimulationFrame.ReadTopValues(frameCount).Average(),
        };
}
