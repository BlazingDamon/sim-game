namespace Main;
internal class ConsoleUtils
{
    public static (int Width, int Height) GetWidthAndHeight()
    {
    RestartRender:
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;
        if (OperatingSystem.IsWindows())
        {
            try
            {
                if (Console.BufferHeight != height) Console.BufferHeight = height;
                if (Console.BufferWidth != width) Console.BufferWidth = width;
            }
            catch (Exception)
            {
                Console.Clear();
                goto RestartRender;
            }
        }
        return (width, height);
    }
}
