using Main.Menus.Base;

namespace Main.Menus;
internal class MainHelpMenu : Menu
{
    public MainHelpMenu()
    {
        MenuTitle = "   HELP MENU";
        MenuBody = 
            [
                "Controls",
                "Close Menu: [esc]",
                "Pause: [/]",
                "Help Menu: [h]",
                "30x Speed: [.]",
                "1x Speed: [,]",
                "Building Menu: [b]",
                "",
                "Goal of the Game",
                "- At the end of your first year, you should strive to have as many statues hoarded as you can muster!",
                "",
                "Tips",
                "- If your food stockpile falls below 20, your people will stop their non-farming occupations to forage.",
                "- If your food stockpile has at least 10 food per person in your population, travelers will continue to arrive.",
                "- Lumber mills produce wood at a faster rate than quarries produce stone."
            ];
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Escape:
                GameGlobals.MenuStack.Pop();
                GameDebugLogger.WriteLog($"Help menu exited.");
                break;
            default:
                wasKeyHandled = false;
                break;
        }

        return wasKeyHandled;
    }
}
