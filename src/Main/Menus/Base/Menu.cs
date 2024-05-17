namespace Main.Menus.Base;
internal abstract class Menu
{
    public LayoutType Layout { get; set; } = LayoutType.FullScreen;
    public string MenuTitle { get; set; } = "";
    public string[] MenuBody { get; set; } = [];

    public abstract bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None);
}
