﻿using Main.Systems.Jobs.Base;

namespace Main.Systems.Jobs;
internal class MaterialsForageJob : BaseJob
{
    private static readonly string _plainName = "foraging for wood and stone";
    public MaterialsForageJob(PersonEntity assignedPerson) : base(_plainName, assignedPerson)
    {
    }
}
