namespace Main;

internal class GameState
{
    public long FramesPassed { get; set; } = 0;
    public int GameSeed { get; set; } = 1234567890;

    public List<ISimulated> SimulatedEntities { get; } = new();
    public List<ISimulated> Buildings { get; } = new();
    public List<ISimulated> Systems { get; } = new();
    public List<Item> GlobalInventory { get; } = new();

    public Scene CurrentScene { get; set; } = new();
}
