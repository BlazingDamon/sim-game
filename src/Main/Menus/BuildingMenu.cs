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
                "  10 wood",
                "Lumber Mill: [l]",
                "  5 stone",
                "Quarry: [q]",
                "  10 wood, 10 stone"
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
