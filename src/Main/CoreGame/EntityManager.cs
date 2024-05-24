using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class EntityManager
{
    private Dictionary<ulong, Entity> _entities = new();

    public void Register(Entity e)
    {
        _entities[e.Id] = e;
    }
}
