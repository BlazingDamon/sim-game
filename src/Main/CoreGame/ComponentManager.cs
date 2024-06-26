﻿using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class ComponentManager
{
    private Dictionary<Type, List<EntityComponent>> _components = new();
    private List<Tuple<ulong, Type>> _componentsToDelete = new();
    private List<ulong> _entitiesToDelete = new();

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

    public void Register(ulong entityId, params IGameComponent[] components)
    {
        foreach (var component in components)
            Register(entityId, component);
    }

    public Entity RegisterNewEntity(params IGameComponent[] components)
    {
        var entity = GameGlobals.CurrentGameState.Entities.CreateEntity();
        foreach (var component in components)
            Register(entity.Id, component);

        return entity;
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

    public List<EntityComponent> GetEntityComponents<T>() where T : IGameComponent
    {
        if (_components.TryGetValue(typeof(T), out var componentList))
        {
            return componentList;
        }
        else
        {
            _components[typeof(T)] = new();
            return _components[typeof(T)];
        }
    }

    public List<EntityComponent> GetEntityComponents<T>(Func<EntityComponent, bool> predicate) where T : IGameComponent
    {
        if (_components.TryGetValue(typeof(T), out var componentList))
        {
            return componentList.Where(predicate).ToList();
        }
        else
        {
            _components[typeof(T)] = new();
            return _components[typeof(T)];
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

    public void DeleteComponent<T>(ulong entityId) where T : IGameComponent
    {
        _componentsToDelete.Add(new Tuple<ulong, Type>(entityId, typeof(T)));
    }

    public void DeleteEntityComponents(ulong entityId)
    {
        _entitiesToDelete.Add(entityId);
    }

    public void FinalizeFrame()
    {
        foreach (Tuple<ulong, Type> tuple in _componentsToDelete)
        {
            int index = _components[tuple.Item2].FindIndex(x => x.EntityId == tuple.Item1);
            if (index >= 0)
                _components[tuple.Item2].RemoveAt(index);
        }

        _componentsToDelete = [];

        foreach (ulong entityId in _entitiesToDelete)
        {
            foreach (List<EntityComponent> componentList in _components.Values)
            {
                int index = componentList.FindIndex(x => x.EntityId == entityId);
                if (index >= 0)
                    componentList.RemoveAt(index);
            }
        }

        _entitiesToDelete = [];
    }
}
