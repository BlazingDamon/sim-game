using Main.CoreGame.Base;
using Main.Items.Food.Base;

namespace Main.Systems.Hunger;
internal class HungerSystem : GameSystem
{
    public HungerSystem() : base(typeof(Components.Health), typeof(Components.Hunger))
    {
    }

    public override void RunSimulationFrame()
    {
        foreach (var healthPair in _componentDictionary[typeof(Components.Health)])
        {
            var hungerPair = _componentDictionary[typeof(Components.Hunger)].Single(x => x.Item1 == healthPair.Item1);

            Components.Health health = (Components.Health)healthPair.Item2;
            Components.Hunger hunger = (Components.Hunger)hungerPair.Item2;

            if (health.IsAlive)
            {
                if (health.IsEntityAgeDayPassedSinceLastFrame())
                {
                    hunger.HungerPoints = Math.Min(100, hunger.HungerPoints+ 10);

                    TryToEat(hunger);

                    if (hunger.HungerPoints > 50)
                    {
                        health.HealthPoints = Math.Max(0, health.HealthPoints - 10);
                    }
                }
            }
        }
    }

    protected void TryToEat(Components.Hunger hunger)
    {
        if (hunger.HungerPoints > 30)
        {
            FoodItem? firstFood = GameGlobals.CurrentGameState.GlobalInventory.OfType<FoodItem>().FirstOrDefault();

            if (firstFood is not null)
            {
                hunger.HungerPoints = Math.Max(0, hunger.HungerPoints - firstFood.HungerRestored + GameRandom.NextInt(2));
                GameGlobals.CurrentGameState.GlobalInventory.Remove(firstFood);
            }
        }
    }
}
