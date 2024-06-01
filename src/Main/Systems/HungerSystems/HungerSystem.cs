using Main.Components;
using Main.CoreGame.Base;

namespace Main.Systems.HungerSystems;
internal class HungerSystem : GameSystem
{
    public HungerSystem() : base(typeof(Health), typeof(Hunger))
    {
    }

    public override void RunSimulationFrame()
    {
        foreach (var hungerPair in GetComponents<Hunger>())
        {
            var healthPair = GetComponents<Health>().Single(x => x.EntityId == hungerPair.EntityId);

            Health health = healthPair.Get<Health>();
            Hunger hunger = hungerPair.Get<Hunger>();

            if (health.IsAlive)
            {
                if (health.IsEntityAgeDayPassedSinceLastFrame())
                {
                    hunger.HungerPoints = Math.Min(100, hunger.HungerPoints + 10);

                    TryToEat(hunger);

                    if (hunger.HungerPoints > 50)
                    {
                        health.HealthPoints = Math.Max(0, health.HealthPoints - 10);
                    }
                }
            }
        }
    }

    protected void TryToEat(Hunger hunger)
    {
        if (hunger.HungerPoints > 30)
        {
            EntityComponent? firstConsumable = GameGlobals.CurrentGameState.Components.GetEntityComponents<Consumable>().FirstOrDefault();

            if (firstConsumable is not null)
            {
                hunger.HungerPoints = Math.Max(0, hunger.HungerPoints - firstConsumable.Get<Consumable>().HungerRestored + GameRandom.NextInt(2));
                GameGlobals.CurrentGameState.Entities.DeleteEntity(firstConsumable.EntityId);
            }
        }
    }
}
