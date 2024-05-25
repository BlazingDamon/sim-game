using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class ComponentManager
{
    private Dictionary<Type, List<EntityComponent>> _components = new();

    public void Register(ulong entityId, IGameComponent component)
    {
        if (_components.TryGetValue(component.GetType(), out var componentList))
        {
            componentList.Add(new EntityComponent(entityId, component));
        }
        else
        {
            _components[component.GetType()] = [new EntityComponent(entityId, component)];
        }
    }

    public T? GetGameComponent<T>(ulong entityId) where T : IGameComponent
    {
        return (T?)_components[typeof(T)].FirstOrDefault(x => x.EntityId == entityId)?.Component;
    }

    public List<EntityComponent> GetEntityComponents(Type type)
    {
        if (_components.TryGetValue(type, out var componentList))
        {
            return componentList;
        }
        else
        {
            _components[type] = new();
            return _components[type];
        }
    }

    public List<EntityComponents> GetEntitiesWithMatchingComponents(params Type[] types)
    {
        if (types.Length == 0)
            return [];

        List<List<EntityComponent>> listOfLists = new();

        foreach (var type in types)
        {
            if (_components.TryGetValue(type, out List<EntityComponent>? componentList))
            {
                listOfLists.Add(componentList);
            }
        }

        return listOfLists
            .SelectMany(x => x)
            .GroupBy(x => x.EntityId)
            .Select(n => new EntityComponents(n.Key, n.Select(i => i.Component).ToList()))
            .Where(x => x.Components.Count() == types.Length)
            .ToList();
    }

}
