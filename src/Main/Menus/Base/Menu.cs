namespace Main.Menus.Base;
internal abstract class Menu
{
    public LayoutType Layout { get; set; } = LayoutType.FullScreen;
    public int? MenuWidth { get; set; } = default;
    public string MenuTitle { get; set; } = "";
    public string[] MenuBody { get; set; } = [];

    public int CurrentlySelectedRow = 0;

    public abstract bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None);

    public virtual string[] BakeMenuBody() { return MenuBody; }
}
