namespace Main.CoreGame.Base;
internal abstract class GameSystem : ISimulated
{
    protected Dictionary<Type, List<(ulong, IGameComponent)>> _componentDictionary = new();

    public GameSystem(params Type[] type)
    {
        foreach (Type t in type)
        {
            _componentDictionary[t] = GameGlobals.CurrentGameState.Components.GetComponents(t);
        }
    }

    public abstract void RunSimulationFrame();
}
