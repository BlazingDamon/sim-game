using Main.CoreGame;

namespace Main;

internal class GameState
{
    public long FramesPassed { get; set; } = 0;
    public int GameSeed { get; set; } = 1234567890;

    public EntityManager Entities { get; } = new();
    public ComponentManager Components { get; } = new();
    public GameSystemManager Systems { get; } = new();

    public Scene CurrentScene { get; set; } = new();

    public GameLogger GameLogger { get; set; } = new();
}
