using Main.Menus.Base;

namespace Main.Menus;
internal class VictoryScreenMenu : Menu
{
    public int NumberOfStatues { get; init; }

    public VictoryScreenMenu(int numberOfStatues)
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
                "Nice work, traveler!",
                "",
                $"Your village was able to hoard a whopping {(NumberOfStatues == 1 ? "1 statue" : $"{NumberOfStatues} statues")}!",
                "",
            ];

        string flavorText = NumberOfStatues switch
        {
            0 => "Although, perhaps, on second thought maybe you were working towards a different goal?",
            < 5 => "Congratulations on honing your craft!",
            >= 5 and < 15 => "Nice work, that's a great collection you've started!",
            >= 15 and < 30 => "That's a lot of statues!",
            >= 30 and < 50 => "Where do you keep all of these? Wow!",
            >= 50 and < 80 => "A true treasure hoard, to be sure. Incredible work!",
            >= 80 and < 120 => "Keep working this hard and you'll be world reknowned!",
            >= 120 => "How did you... do that? Truly impressive."

        };

        menuBody.Add(flavorText);
        menuBody.Add("");
        menuBody.Add("");
        menuBody.Add("Press [esc] or [enter] to exit the game...");

        MenuBody = menuBody.ToArray();
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Enter:
            case ConsoleKey.Escape:
                GameGlobals.IsGameRunning = false;
                break;
            default:
                break;
        }

        return wasKeyHandled;
    }
}
