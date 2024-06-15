using Main.Components.Enums;
using Main.CoreGame.Base;

namespace Main.Components;
internal class BuildingMaterial(MaterialType materialType) : IGameComponent
{
    public MaterialType MaterialType { get; set; } = materialType;
}
