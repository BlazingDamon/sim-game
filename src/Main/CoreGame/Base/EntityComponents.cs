namespace Main.CoreGame.Base;
internal class EntityComponents
{
    public EntityComponents(ulong entityId, IEnumerable<IGameComponent> component)
    {
        EntityId = entityId;
        Components = component.ToList();
    }

    public ulong EntityId { get; init; }
    public List<IGameComponent> Components { get; init; }

    public T Get<T>() where T : IGameComponent
    {
        return Components.OfType<T>().Single();
    }
}
