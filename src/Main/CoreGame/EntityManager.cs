using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class EntityManager
{
    private Dictionary<ulong, Entity> _entities = new();
    private ulong SequenceNumber { get; set; } = 0;

    private List<ulong> _entitiesToDelete = new();

    public Entity CreateEntity()
    {
        var e = new Entity
        {
            Id = GenerateEntityId()
        };

        Register(e);

        return e;
    }

    public ulong GenerateEntityId() => SequenceNumber++;

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
        _entitiesToDelete.Add(id);
    }

    public void FinalizeFrame()
    {
        foreach (ulong entityId in _entitiesToDelete)
        {
            GameGlobals.CurrentGameState.Components.DeleteEntityComponents(entityId);
            _entities.Remove(entityId);
        }

        _entitiesToDelete = [];
    }

    private void Register(Entity e)
    {
        _entities[e.Id] = e;
    }
}
