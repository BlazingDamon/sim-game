using Main.CoreGame.Base;
using System.Collections.Generic;

namespace Main.CoreGame;
internal class ComponentManager
{
    private Dictionary<Type, List<(ulong, IGameComponent)>> _components = new();

    public void Register(ulong entityId, IGameComponent component)
    {
        if (_components.TryGetValue(component.GetType(), out var componentList))
        {
            componentList.Add((entityId, component));
        }
        else
        {
            _components[component.GetType()] = [(entityId, component)];
        }
    }

    public T? GetGameComponent<T>(ulong entityId) where T : IGameComponent
    {
        return (T)_components[typeof(T)].FirstOrDefault(x => x.Item1 == entityId).Item2;
    }

    public List<(ulong, IGameComponent)> GetComponents(Type type)
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

    public List<(ulong, List<IGameComponent>)> GetEntitiesWithMatchingComponents(params Type[] types)
    {
        if (types.Length == 0)
            return [];

        List<List<(ulong, IGameComponent)>> listOfLists = new();

        foreach (var type in types)
        {
            if (_components.TryGetValue(type, out List<(ulong, IGameComponent)>? componentList))
            {
                listOfLists.Add(componentList);
            }
        }

        return listOfLists
            .SelectMany(x => x)
            .GroupBy(x => x.Item1)
            .Select(n => (n.Key, n.Select(i => i.Item2).ToList()))
            .Where(x => x.Item2.Count() == types.Length)
            .ToList();
    }

}
