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
            >= 5 and < 25 => "Nice work, that's a great collection you've started!",
            >= 25 and < 50 => "That's a lot of statues!",
            >= 50 and < 75 => "Where do you keep all of these? Wow!",
            >= 75 and < 100 => "A true treasure hoard, to be sure. Incredible work!",
            >= 100 and < 200 => "Keep working this hard and you'll be world reknowned!",
            >= 200 => "How did you... do that? Truly impressive."

        };

        menuBody.Add(flavorText);
        menuBody.Add("");
        menuBody.Add("");
        menuBody.Add("Press [enter] to exit the game...");

        MenuBody = menuBody.ToArray();
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Enter:
                GameGlobals.IsGameRunning = false;
                break;
            default:
                break;
        }

        return wasKeyHandled;
    }
}
