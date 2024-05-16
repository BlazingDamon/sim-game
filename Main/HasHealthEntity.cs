namespace Main;
internal abstract class HasHealthEntity : SimulatedEntity
{
    public int Health { get; set; }
    public int MaxHealth { get; set; } = 100;

    public override void RunSimulationFrame()
    {
        base.RunSimulationFrame();

        if (IsAlive)
        {
            if (IsEntityAgeDayPassedSinceLastFrame())
            {
                Health = Math.Min(MaxHealth, Health + 2);
            }
        }
    }
}
