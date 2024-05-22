using Main.Menus.Base;

namespace Main.Menus;
internal class HalfwayMenu : Menu
{
    public int NumberOfStatues { get; init; }

    public HalfwayMenu(int numberOfStatues)
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
                "You are half way to 180 days, traveler!",
                "",
                $"Remember, your goal is to hoard as many statues as you can before the time's up!",
                "",
                $"Right now, you have {(NumberOfStatues == 1 ? "1 statue" : $"{NumberOfStatues} statues")}!",
                "",
            ];

        menuBody.Add("");
        menuBody.Add("Press [enter] to continue the game...");

        MenuBody = menuBody.ToArray();
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Enter:
                GameGlobals.MenuStack.Pop();
                GameGlobals.IsSimulationRunning = GameGlobals.UserPrefersSimulationRunning;
                break;
            default:
                break;
        }

        return wasKeyHandled;
    }
}
