using Main.CoreGame;
using Main.CoreGame.Base;

namespace Main;

internal class GameState
{
    public long FramesPassed { get; set; } = 0;
    public int GameSeed { get; set; } = 1234567890;

    public EntityManager Entities { get; } = new();
    public ComponentManager Components { get; } = new();
    public GameSystemManager Systems2 { get; } = new();
    public List<ISimulated> SimulatedEntities { get; } = new();
    public List<ISimulated> Buildings { get; } = new();
    public List<ISimulated> Systems { get; } = new();
    public List<BaseItem> GlobalInventory { get; } = new();

    public Scene CurrentScene { get; set; } = new();

    public GameLogger GameLogger { get; set; } = new();
}
