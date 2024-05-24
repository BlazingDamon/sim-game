using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class GameSystemManager
{
    private List<GameSystem> _gameSystems = new();

    public void Register(GameSystem gameSystem)
    {
        _gameSystems.Add(gameSystem);
    }
    
    public IEnumerable<GameSystem> GetGameSystems() => _gameSystems;
}
