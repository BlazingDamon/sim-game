using Main.CoreGame.Base;

namespace Main.Components;
internal class Consumable(int hungerRestored = 0) : IGameComponent
{
    public int HungerRestored { get; set; } = hungerRestored;
    public bool IsConsumed { get; set; } = false;
}
