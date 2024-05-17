namespace Main;
internal class OpeningScene : Scene
{
    private static readonly string[] _defaultMapText =
        [
            "Test a key: [anykey]",
            "Pause: [p]",
            "50x Speed: [.]",
            "1x Speed: [,]",
            "Test random int generation: [r]",
            "Quit: [escape]",
        ];

    public OpeningScene()
    {
        MapText = _defaultMapText;
    }
}
