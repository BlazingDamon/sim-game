﻿using Main.Components;
using Main.Components.Enums;
using Main.Items;
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
                if (ItemSearcher.TryUseBuildingMaterial(MaterialType.Wood, 10))
                {
                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a farm.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new Building(BuildingType.Farm));
                }
                break;
            case ConsoleKey.L:
                if (ItemSearcher.TryUseBuildingMaterial(MaterialType.Stone, 5))
                {
                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a lumber mill.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new Building(BuildingType.LumberMill));
                }
                break;
            case ConsoleKey.Q:
                if (ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Wood, 10) && 
                    ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Stone, 10))
                {
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Wood, 10);
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Stone, 10);

                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a quarry.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new Building(BuildingType.Quarry));
                }
                break;
            case ConsoleKey.S:
                if (ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Wood, 30) &&
                    ItemSearcher.CheckBuildingMaterialCountIsAtLeast(MaterialType.Stone, 30))
                {
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Wood, 30);
                    ItemSearcher.TryUseBuildingMaterial(MaterialType.Stone, 30);

                    GameGlobals.CurrentGameState.GameLogger.WriteLog("Your villagers have constructed a statue workshop.");
                    GameGlobals.CurrentGameState.Components.RegisterNewEntity(new Building(BuildingType.StatueWorkshop));
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
