using Main.Menus.Base;

namespace Main.Menus;
internal class PartWayMenu : Menu
{
    public int NumberOfStatues { get; init; }

    public PartWayMenu(int numberOfStatues)
    {
        NumberOfStatues = numberOfStatues;
        TextLayout = TextLayoutType.TopCenter;
        MenuTitle = "";
        List<string> menuBody =
            [
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "You are 30 days away from 180 days, traveler!",
                "",
                $"Remember, your goal is to hoard as many statues as you can before the time's up!",
                "",
                $"Right now, you have {(NumberOfStatues == 1 ? "1 statue" : $"{NumberOfStatues} statues")}!",
                "",
            ];

        menuBody.Add("");
        menuBody.Add("Press [esc] or [enter] to continue the game...");

        MenuBody = menuBody.ToArray();
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Enter:
            case ConsoleKey.Escape:
                GameGlobals.MenuStack.Pop();
                GameGlobals.IsSimulationRunning = GameGlobals.UserPrefersSimulationRunning;
                break;
            default:
                break;
        }

        return wasKeyHandled;
    }
}
