using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class EntityManager
{
    private Dictionary<ulong, Entity> _entities = new();
    public int SequenceNumber { get; set; } = 0;

    public Entity CreateEntity()
    {
        var e = new Entity
        {
            Id = GenerateEntityId()
        };

        Register(e);

        return e;
    }

    public ulong GenerateEntityId() =>
        ((ulong)GameGlobals.CurrentGameState.FramesPassed * 1000) + (ulong)SequenceNumber++;

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

    public void DeleteEntity(ulong id)
    {
        GameGlobals.CurrentGameState.Components.DeleteEntityComponents(id);
        _entities.Remove(id);
    }

    private void Register(Entity e)
    {
        _entities[e.Id] = e;
    }
}
