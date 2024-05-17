namespace Main;
internal class OpeningScene : Scene
{
    private static readonly string[] _defaultMapText =
        [
            "Menu: [esc]",
            "Pause: [p]",
            "Help Menu: [h]",
            "50x Speed: [.]",
            "1x Speed: [,]",
        ];

    public OpeningScene()
    {
        MapText = _defaultMapText;
    }
}
