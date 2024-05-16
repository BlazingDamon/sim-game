namespace Main;
internal class GameState
{
    public long FramesPassed { get; set; } = 0;
    public string LastKeyPressed = string.Empty;
    public int GameSeed { get; set; } = 1234567890;

    public List<ISimulated> SimulatedEntities { get; } = new List<ISimulated>();
}
