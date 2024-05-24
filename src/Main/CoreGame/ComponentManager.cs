using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class ComponentManager
{
    private Dictionary<Type, List<(ulong, IComponent)>> _components = new();

    public void Register(ulong entityId, IComponent component)
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

    public List<(ulong, IComponent)> GetComponents(Type type)
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
}
