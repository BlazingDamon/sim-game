namespace Main;

internal abstract class HasHungerEntity : HasHealthEntity
{
    public int Hunger { get; set; }

    public override void RunSimulationFrame()
    {
        base.RunSimulationFrame();

        if (IsAlive)
        {
            if (IsEntityAgeDayPassedSinceLastFrame())
            {
                Hunger = Math.Min(100, Hunger + 10);

                TryToEat();
                
                if (Hunger > 50)
                {
                    Health = Math.Max(0, Health - 10);
                }
            }
        }
    }

    protected virtual void TryToEat()
    {
        GameDebugLogger.WriteLog($"TryToEat. {Hunger},{Health}");
    }
}
