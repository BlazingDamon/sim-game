using Main.Components;
using Main.CoreGame.Base;
using Main.Items.Food;
using Main.Items.Material;

namespace Main.Systems.JobSystems;
internal class ForageSystem : GameSystem
{
    public ForageSystem() : base(typeof(Health), typeof(Job)) { }

    public override void RunSimulationFrame()
    {

        foreach (var healthComponent in GetComponents<Health>())
        {
            Health health = healthComponent.Get<Health>();
            if (health.IsAlive)
            {
                if (health.IsEntityAgeDayPassedSinceLastFrame())
                {
                    var jobComponent = GetComponents<Job>().FirstOrDefault(x => x.EntityId == healthComponent.EntityId)?.Get<Job>();
                    if (jobComponent is null)
                        continue;

                    if (jobComponent.CurrentJob is FoodForageJobECS && GameRandom.NextInt(3) > 1)
                    {
                        GameGlobals.CurrentGameState.GlobalInventory.Add(new FarmedFoodItem());
                    }
                    else if (jobComponent.CurrentJob is MaterialsForageJobECS)
                    {
                        int random = GameRandom.NextInt(100);
                        if (random > 75)
                        {
                            GameGlobals.CurrentGameState.GlobalInventory.Add(new StoneItem());
                        }
                        else if (random > 25)
                        {
                            GameGlobals.CurrentGameState.GlobalInventory.Add(new WoodItem());
                        }
                    }
                }
            }
        }
    }
}
