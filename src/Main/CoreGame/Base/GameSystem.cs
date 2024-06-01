namespace Main.CoreGame.Base;
internal abstract class GameSystem : ISimulated
{
    private Dictionary<Type, List<EntityComponent>> _componentDictionary = new();

    public GameSystem(params Type[] types)
    {
        foreach (Type t in types)
        {
            _componentDictionary[t] = GameGlobals.CurrentGameState.Components.GetEntityComponents(t);
        }
    }

    public abstract void RunSimulationFrame();

    public List<EntityComponent> GetComponents<T>() where T : IGameComponent
    {
        return _componentDictionary[typeof(T)];
    }
}
