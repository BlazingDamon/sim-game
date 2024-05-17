using Main.Menus.Base;

namespace Main;
internal class GameGlobals
{
    public static bool IsGameRunning { get; set; }
    public static bool IsSimulationRunning { get; set; }
    public static bool UserPrefersSimulationRunning { get; set; }


    public static DateTime PreviousRender { get; set; } = DateTime.Now;


    public static GameState CurrentGameState { get; set; } = new();

    public static Stack<Menu> MenuStack { get; set; } = new();
    public static int GameSpeed { get; set; } = 1;
}
