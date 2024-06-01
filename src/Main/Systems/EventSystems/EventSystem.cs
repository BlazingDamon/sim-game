using Main.Items;
using Main.Items.Decorative;
using Main.Menus;

namespace Main.Systems.EventSystems;
internal class EventSystem : ISimulated
{
    private static readonly long FRAME_AT_150_DAYS = GameConstants.SECONDS_IN_DAY * 150 / GameConfig.TimePerFrameInSeconds;
    private static readonly long FRAME_AT_180_DAYS = GameConstants.SECONDS_IN_DAY * 180 / GameConfig.TimePerFrameInSeconds;

    public void RunSimulationFrame()
    {
        if (GameGlobals.CurrentGameState.FramesPassed == FRAME_AT_150_DAYS)
        {
            GameGlobals.IsSimulationRunning = false;
            GameGlobals.MenuStack.Push(new PartWayMenu(ItemSearcherOld.GetItemCount<StatueItem>()));
        }

        if (GameGlobals.CurrentGameState.FramesPassed == FRAME_AT_180_DAYS)
        {
            GameGlobals.IsSimulationRunning = false;
            GameGlobals.MenuStack.Push(new VictoryScreenMenu(ItemSearcherOld.GetItemCount<StatueItem>()));
        }
    }
}
