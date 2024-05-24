using Main.CoreGame.Base;
using Main.Items.Decorative;
using Main.Items;
using Main.Menus;

namespace Main.Systems.Events;
internal class EventSystemECS : GameSystem
{
    private static readonly long FRAME_AT_150_DAYS = (GameConstants.SECONDS_IN_DAY * 100) / GameConfig.TimePerFrameInSeconds;
    private static readonly long FRAME_AT_180_DAYS = (GameConstants.SECONDS_IN_DAY * 110) / GameConfig.TimePerFrameInSeconds;

    public override void RunSimulationFrame()
    {
        if (GameGlobals.CurrentGameState.FramesPassed == FRAME_AT_150_DAYS)
        {
            GameGlobals.IsSimulationRunning = false;
            GameGlobals.MenuStack.Push(new PartWayMenu(ItemSearcher.GetItemCount<StatueItem>()));
        }

        if (GameGlobals.CurrentGameState.FramesPassed == FRAME_AT_180_DAYS)
        {
            GameGlobals.IsSimulationRunning = false;
            GameGlobals.MenuStack.Push(new VictoryScreenMenu(ItemSearcher.GetItemCount<StatueItem>()));
        }
    }
}
