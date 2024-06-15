using Main.Components;
using Main.Components.Enums;
using Main.CoreGame.Base;
using Main.Entities;

namespace Main.Systems.JobSystems;
internal class ForageSystem : GameSystem
{
    public ForageSystem() : base(typeof(Health), typeof(Employment)) { }

    public override void RunSimulationFrame()
    {
        foreach (var healthComponent in GetComponents<Health>())
        {
            Health health = healthComponent.Get<Health>();
            if (health.IsAlive)
            {
                if (health.IsEntityAgeDayPassedSinceLastFrame())
                {
                    var jobComponent = GetComponents<Employment>().FirstOrDefault(x => x.EntityId == healthComponent.EntityId)?.Get<Employment>();
                    if (jobComponent is null || jobComponent.CurrentJob is null)
                        continue;

                    if (jobComponent.CurrentJob is FoodForageJob && GameRandom.NextInt(3) > 1)
                    {
                        EntityGen.FoodItem(25);
                    }
                    else if (jobComponent.CurrentJob is MaterialsForageJob)
                    {
                        int random = GameRandom.NextInt(100);
                        if (random > 75)
                        {
                            EntityGen.BuildingMaterialItem(MaterialType.Stone);
                        }
                        else if (random > 25)
                        {
                            EntityGen.BuildingMaterialItem(MaterialType.Wood);
                        }
                    }
                }
            }
        }
    }
}
