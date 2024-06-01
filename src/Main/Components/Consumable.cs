using Main.CoreGame.Base;

namespace Main.Components;
internal class Consumable(int hungerRestored) : IGameComponent
{
    public int HungerRestored { get; set; } = hungerRestored;
}
