using Main.Entities.Buildings;
using Main.Items;
using Main.Items.Material;
using Main.Menus.Base;

namespace Main.Menus;
internal class BuildingMenu : Menu
{
    public BuildingMenu()
    {
        Layout = LayoutType.RightThird;
        MenuTitle = "   BUILDING";
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
                if (ItemSearcher.TryUseItem<WoodItem>(10))
                {
                    GameGlobals.CurrentGameState.Buildings.Add(new FarmBuilding());
                }
                break;
            case ConsoleKey.L:
                if (ItemSearcher.TryUseItem<StoneItem>(5))
                {
                    GameGlobals.CurrentGameState.Buildings.Add(new LumberMillBuilding());
                }
                break;
            case ConsoleKey.Q:
                if (ItemSearcher.CheckItemCountIsAtLeast<WoodItem>(10) && ItemSearcher.CheckItemCountIsAtLeast<StoneItem>(10))
                {
                    ItemSearcher.TryUseItem<WoodItem>(10);
                    ItemSearcher.TryUseItem<StoneItem>(10);
                    GameGlobals.CurrentGameState.Buildings.Add(new QuarryBuilding());
                }
                break;
            case ConsoleKey.S:
                if (ItemSearcher.CheckItemCountIsAtLeast<WoodItem>(30) && ItemSearcher.CheckItemCountIsAtLeast<StoneItem>(30))
                {
                    ItemSearcher.TryUseItem<WoodItem>(30);
                    ItemSearcher.TryUseItem<StoneItem>(30);
                    GameGlobals.CurrentGameState.Buildings.Add(new StatueWorkshopBuilding());
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
