namespace Main.Menus.Base;
internal abstract class Menu
{
    public LayoutType Layout { get; set; } = LayoutType.FullScreen;
    public TextLayoutType TextLayout { get; set; } = TextLayoutType.TopLeft;
    public int? MenuWidth { get; set; } = default;
    public string MenuTitle { get; set; } = "";
    public string MenuFooter { get; set; } = "Press [esc] to close this menu";
    public string[] MenuBody { get; set; } = [];

    public int CurrentlySelectedRow = 0;

    public abstract bool HandleInput(ConsoleKey pressedKey = ConsoleKey.None);

    public virtual string[] BakeMenuBody() { return MenuBody; }
}
