using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
