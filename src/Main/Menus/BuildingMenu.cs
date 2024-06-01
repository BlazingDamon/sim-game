using Main.Entities.Buildings;
using Main.Entities.Materials;
using Main.Items;
using Main.Items.Material;
using Main.Menus.Base;

namespace Main.Menus;
internal class BuildingMenu : Menu
{
    public BuildingMenu()
    {
        Layout = LayoutType.RightThird;
        MenuTitle = "   BUILDING MENU";
        MenuBody =
            [
                "Farm: [f]",
                "  Build Cost: 10 wood",
                "Lumber Mill: [l]",
                "  Build Cost: 5 stone",
                "Quarry: [q]",
                "  Build Cost: 10 wood, 10 stone",
                "Statue Workshop: [s]",
                "  Build Cost: 30 wood, 30 stone",
                "  Unit Cost: 10 wood, 10 stone",
                "  Statues won't be built until stockpile",
                "   of wood and stone both reach 40.",
            ];
    }

    public override bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None)
    {
        bool wasKeyHandled = true;
        switch (pressedKey)
        {
            case ConsoleKey.Escape:
                GameGlobals.MenuStack.Pop();
                GameDebugLogger.WriteLog($"Building menu exited.");
                break;
            case ConsoleKey.F:
                if (ItemSearcherOld.TryUseItem<WoodItem>(10))
                {
                    GameGlobals.CurrentGameState.GameLogger.WriteLog("OLD: Your villagers have constructed a farm.");
                    GameGlobals.CurrentGameState.Buildings.Add(new FarmBuilding());
                }
                if (ItemSearcher.TryUseBuildingMaterial(MaterialType.Wood, 10))
                {
                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a farm.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new BuildingECS(BuildingType.Farm));
                }
                break;
            case ConsoleKey.L:
                if (ItemSearcherOld.TryUseItem<StoneItem>(5))
                {
                    GameGlobals.CurrentGameState.GameLogger.WriteLog("OLD: Your villagers have constructed a lumber mill.");
                    GameGlobals.CurrentGameState.Buildings.Add(new LumberMillBuilding());
                }
                if (ItemSearcher.TryUseBuildingMaterial(MaterialType.Stone, 5))
                {
                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a lumber mill.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new BuildingECS(BuildingType.LumberMill));
                }
                break;
            case ConsoleKey.Q:
                if (ItemSearcherOld.CheckItemCountIsAtLeast<WoodItem>(10) && ItemSearcherOld.CheckItemCountIsAtLeast<StoneItem>(10))
                {
                    ItemSearcherOld.TryUseItem<WoodItem>(10);
                    ItemSearcherOld.TryUseItem<StoneItem>(10);

                    GameGlobals.CurrentGameState.GameLogger.WriteLog("OLD: Your villagers have constructed a quarry.");
                    GameGlobals.CurrentGameState.Buildings.Add(new QuarryBuilding());
                }
                if (ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Wood, 10) && 
                    ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Stone, 10))
                {
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Wood, 10);
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Stone, 10);

                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a quarry.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new BuildingECS(BuildingType.Quarry));
                }
                break;
            case ConsoleKey.S:
                if (ItemSearcherOld.CheckItemCountIsAtLeast<WoodItem>(30) && ItemSearcherOld.CheckItemCountIsAtLeast<StoneItem>(30))
                {
                    ItemSearcherOld.TryUseItem<WoodItem>(30);
                    ItemSearcherOld.TryUseItem<StoneItem>(30);

                    GameGlobals.CurrentGameState.GameLogger.WriteLog("OLD: Your villagers have constructed a statue workshop.");
                    GameGlobals.CurrentGameState.Buildings.Add(new StatueWorkshopBuilding());
                }
                if (ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Wood, 30) &&
                    ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Stone, 30))
                {
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Wood, 30);
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Stone, 30);

                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a statue workshop.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new BuildingECS(BuildingType.StatueWorkshop));
                }
                break;
            default:
                wasKeyHandled = false;
                break;
        }

        return wasKeyHandled;
    }

    public override string[] BakeMenuBody()
    {
        return base.BakeMenuBody();
    }
}
