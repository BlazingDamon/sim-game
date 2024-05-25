using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class EntityManager
{
    private Dictionary<ulong, Entity> _entities = new();

    public void Register(Entity e)
    {
        _entities[e.Id] = e;
    }

    public Entity Query(ulong id)
    {
        return _entities[id];
    }

    public List<EntityComponent> QueryByType(Type type)
    {
        return GameGlobals.CurrentGameState.Components.GetEntityComponents(type);
    } 

    public List<EntityComponents> QueryByTypes(params Type[] types)
    {
        return GameGlobals.CurrentGameState.Components.GetEntitiesWithMatchingComponents(types); 
    }
}
