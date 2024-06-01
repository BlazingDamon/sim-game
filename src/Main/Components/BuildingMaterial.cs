using Main.CoreGame.Base;
using Main.Entities.Materials;

namespace Main.Components;
internal class BuildingMaterial(MaterialType materialType) : IGameComponent
{
    public MaterialType MaterialType { get; set; } = materialType;
}
