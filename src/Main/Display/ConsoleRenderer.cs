using Main.Menus.Base;
using System.Text;

namespace Main.Display;
internal class ConsoleRenderer
{
    public static void Render()
    {
        Console.CursorVisible = false;
        var sceneText = GameGlobals.CurrentGameState.CurrentScene.SceneText;
        var overviewText = GameBaker.BakedOverview;
        var currentMenuExists = GameGlobals.MenuStack.TryPeek(out Menu? menu);

        var (consoleWidth, consoleHeight) = ConsoleUtils.GetWidthAndHeight();
        int heightCutoff = Math.Min(consoleHeight - 8, (int)(consoleHeight * .80));
        int widthCutoff = Math.Min(40, (int)(consoleWidth * .30));

        StringBuilder sb = new(consoleWidth * consoleHeight);

        for (int consoleLine = 0; consoleLine < consoleHeight; consoleLine++)
        {
            if (OperatingSystem.IsWindows() && consoleLine == consoleHeight - 1)
            {
                break;
            }

            if (currentMenuExists)
            {
                if (menu!.Layout == LayoutType.FullScreen)
                {
                    sb.Append(RenderOneLineOfFullScreenMenu(menu, consoleWidth, consoleHeight, consoleLine));
                }
                else if (menu!.Layout == LayoutType.RightFixed || menu!.Layout == LayoutType.RightThird)
                {
                    sb.Append(RenderOneLineOfRightScreenMenuLayout(menu, overviewText, sceneText, consoleWidth, consoleHeight, heightCutoff, widthCutoff, consoleLine));
                }
            }
            else
            {
                sb.Append(RenderOneLineOfDefaultLayout(overviewText, sceneText, consoleWidth, consoleHeight, heightCutoff, widthCutoff, consoleLine));
            }

            if (!OperatingSystem.IsWindows() && consoleLine < consoleHeight - 1)
            {
                sb.AppendLine();
            }

        }

        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
    }

    private static string RenderOneLineOfFullScreenMenu(Menu menu, int width, int height, int consoleLine)
    {
        StringBuilder sb = new(width);

        for (int consoleCharacter = 0; consoleCharacter < width; consoleCharacter++)
        {
            ConsoleTextBoxRenderer.RenderTextBox(menu.MenuBody, width, consoleLine, sb, consoleCharacter, 0, 0, height - 2, width, BorderType.SolidBorder, menu.MenuTitle, textTopMargin: 1, textLeftMargin: 2, textLayout: menu.TextLayout);
        }

        return sb.ToString();
    }

    private static string RenderOneLineOfRightScreenMenuLayout(Menu menu, string[] overviewText, string[] sceneText, int consoleWidth, int consoleHeight, int heightCutoff, int widthCutoff, int consoleLine)
    {
        StringBuilder sb = new(consoleWidth);
        int mainDisplayScroll = ClampMainDisplayScroll(sceneText.Length, heightCutoff - 3);

        for (int consoleCharacter = 0; consoleCharacter < consoleWidth; consoleCharacter++)
        {
            if (RenderOverviewCorner(overviewText, consoleWidth, consoleHeight, heightCutoff, widthCutoff, consoleLine, sb, consoleCharacter))
                continue;

            if (RenderLogsCorner(consoleWidth, consoleHeight, heightCutoff, widthCutoff, consoleLine, sb, consoleCharacter))
                continue;

            int menuWidth;
            if (menu.Layout == LayoutType.RightThird)
                menuWidth = (int)(consoleWidth * 0.3333);
            else if (menu.Layout == LayoutType.RightFixed && menu.MenuWidth.HasValue)
                menuWidth = consoleWidth - menu.MenuWidth.Value;
            else
                throw new Exception("There is a misconfigured menu layout. Check that MenuWidth is set correctly, or that your LayoutType is handled correctly.");

            if (RenderMainArea(sceneText, consoleWidth, heightCutoff, consoleLine, sb, mainDisplayScroll, consoleCharacter, menuWidth))
                continue;

            // menu area
            if (RenderMenu(menu, consoleWidth, heightCutoff, consoleLine, sb, consoleCharacter, menuWidth))
                continue;

            // if you made it here, this is just extra whitespace to fill out the lines
            char c = ' ';
            sb.Append(char.IsWhiteSpace(c) ? ' ' : c);
        }

        return sb.ToString();
    }

    private static string RenderOneLineOfDefaultLayout(string[] overviewText, string[] sceneText, int width, int height, int heightCutOff, int widthCutoff, int consoleLine)
    {
        StringBuilder sb = new(width);
        int mainDisplayScroll = ClampMainDisplayScroll(sceneText.Length, heightCutOff - 3);

        for (int consoleCharacter = 0; consoleCharacter < width; consoleCharacter++)
        {
            if (RenderOverviewCorner(overviewText, width, height, heightCutOff, widthCutoff, consoleLine, sb, consoleCharacter))
                continue;

            if (RenderLogsCorner(width, height, heightCutOff, widthCutoff, consoleLine, sb, consoleCharacter))
                continue;

            // main area
            if (RenderMainArea(sceneText, width, heightCutOff, consoleLine, sb, mainDisplayScroll, consoleCharacter, menuWidth: 0))
                continue;

            // if you made it here, this is just extra whitespace to fill out the lines
            char c = ' ';
            sb.Append(char.IsWhiteSpace(c) ? ' ' : c);
        }

        return sb.ToString();
    }

    private static bool RenderMenu(Menu menu, int width, int heightCutOff, int consoleLine, StringBuilder sb, int consoleCharacter, int menuWidth)
    {
        if (consoleLine < heightCutOff && consoleCharacter >= 0 && consoleLine >= 0 && consoleCharacter >= (width - menuWidth))
        {
            ConsoleTextBoxRenderer.RenderTextBox(menu.MenuBody, width, consoleLine, sb, consoleCharacter, 0, (width - menuWidth),
                heightCutOff - 1, menuWidth, BorderType.SolidBorder, headerText: menu.MenuTitle, footerText: menu.MenuFooter, textTopMargin: 1, textLeftMargin: 2, textLayout: menu.TextLayout);
            return true;
        }

        return false;
    }

    private static bool RenderMainArea(string[] sceneText, int width, int heightCutOff, int consoleLine, StringBuilder sb, int mainDisplayScroll, int consoleCharacter, int menuWidth)
    {
        if (consoleLine < heightCutOff && consoleCharacter >= 0 && consoleLine >= 0 && consoleCharacter < (width - menuWidth))
        {
            string? headerText = null;
            if (!GameGlobals.IsSimulationRunning)
                headerText = "   PAUSED";
            else if (GameGlobals.GameSpeed > 1)
                headerText = $"   SPEED:{GameGlobals.GameSpeed}x";

            string? footerText = null;
            if (sceneText.Length > heightCutOff - 2)
            {
                if (mainDisplayScroll == 0)
                    footerText = "   Top of page. [j] to scroll down";
                else if (mainDisplayScroll == sceneText.Length - heightCutOff + 3)
                    footerText = "   Bottom of page. [k] to scroll up";
                else
                    footerText = "   [j] to scroll down, [k] to scroll up";
            }

            ConsoleTextBoxRenderer.RenderTextBox(sceneText, width, consoleLine, sb, consoleCharacter, heightOffset: 0, widthOffset: 0,
                heightCutOff - 1, (width - menuWidth), BorderType.SolidBorder, headerText, footerText, bodyTextScrollHeight: mainDisplayScroll);
            return true;
        }

        return false;
    }

    private static bool RenderLogsCorner(int width, int height, int heightCutOff, int widthCutoff, int consoleLine, StringBuilder sb, int consoleCharacter)
    {
        if (consoleLine >= heightCutOff && consoleCharacter >= widthCutoff)
        {
            string[] logs;
            if (GameGlobals.IsDebugModeEnabled)
                logs = GameDebugLogger.ReadLogs(Math.Max(height - heightCutOff - 3, 0));
            else
                logs = GameGlobals.CurrentGameState.GameLogger.ReadLogs(Math.Max(height - heightCutOff - 3, 0));

            var heightOffset = heightCutOff;
            var widthOffset = widthCutoff;
            ConsoleTextBoxRenderer.RenderTextBox(logs, width, consoleLine, sb, consoleCharacter, heightOffset, widthOffset,
                height - heightCutOff - 2, width - widthCutoff, BorderType.SolidBorder, headerText: "   GAME LOGS");
            return true;
        }

        return false;
    }

    private static bool RenderOverviewCorner(string[] overviewText, int width, int height, int heightCutOff, int widthCutoff, int consoleLine, StringBuilder sb, int consoleCharacter)
    {
        if (consoleLine >= heightCutOff && consoleCharacter < widthCutoff)
        {
            var heightOffset = heightCutOff;
            var widthOffset = 0;
            ConsoleTextBoxRenderer.RenderTextBox(overviewText, width, consoleLine, sb, consoleCharacter, heightOffset, widthOffset,
                height - heightCutOff - 2, widthCutoff, BorderType.SolidBorder, headerText: "   OVERVIEW");
            return true;
        }

        return false;
    }

    private static int ClampMainDisplayScroll(int linesOfText, int effectiveHeightOfTextBox)
    {
        int mainDisplayScroll = Math.Clamp(GameGlobals.MainDisplayScrollHeight, 0, Math.Max(0, linesOfText - effectiveHeightOfTextBox));
        GameGlobals.MainDisplayScrollHeight = mainDisplayScroll;
        return mainDisplayScroll;
    }
}
