﻿namespace Main.CoreGame.Base;
internal class EntityComponent
{
    public EntityComponent(ulong entityId, IGameComponent component)
    {
        EntityId = entityId;
        Component = component;
    }

    public ulong EntityId { get; init; }
    public IGameComponent Component { get; init; }
}
