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
            RenderTextInBoxWithOffset(menu.MenuBody, width, consoleLine, sb, consoleCharacter, 0, 0, height - 2, width, BorderType.SolidBorder, menu.MenuTitle, textTopMargin: 1, textLeftMargin: 2, textLayout: menu.TextLayout);
        }

        return sb.ToString();
    }

    private static string RenderOneLineOfRightScreenMenuLayout(Menu menu, string[] overviewText, string[] sceneText, int width, int height, int heightCutOff, int widthCutoff, int consoleLine)
    {
        StringBuilder sb = new(width);
        int mainDisplayScroll = Math.Clamp(GameGlobals.MainDisplayScrollHeight, 0, Math.Max(0, sceneText.Length - heightCutOff + 3));
        GameGlobals.MainDisplayScrollHeight = mainDisplayScroll;

        for (int consoleCharacter = 0; consoleCharacter < width; consoleCharacter++)
        {
            if (RenderOverviewCorner(overviewText, width, height, heightCutOff, widthCutoff, consoleLine, sb, consoleCharacter))
                continue;

            if (RenderLogsCorner(width, height, heightCutOff, widthCutoff, consoleLine, sb, consoleCharacter))
                continue;

            int menuWidth;
            if (menu.Layout == LayoutType.RightThird)
                menuWidth = (int)(width * 0.3333);
            else if (menu.Layout == LayoutType.RightFixed && menu.MenuWidth.HasValue)
                menuWidth = width - menu.MenuWidth.Value;
            else
                throw new Exception("There is a misconfigured menu layout. Check that MenuWidth is set correctly, or that your LayoutType is handled correctly.");

            if (RenderMainArea(sceneText, width, heightCutOff, consoleLine, sb, mainDisplayScroll, consoleCharacter, menuWidth))
                continue;

            // menu area
            if (RenderMenu(menu, width, heightCutOff, consoleLine, sb, consoleCharacter, menuWidth))
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
        int mainDisplayScroll = Math.Clamp(GameGlobals.MainDisplayScrollHeight, 0, Math.Max(0, sceneText.Length - heightCutOff + 3));
        GameGlobals.MainDisplayScrollHeight = mainDisplayScroll;

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
            RenderTextInBoxWithOffset(menu.MenuBody, width, consoleLine, sb, consoleCharacter, 0, (width - menuWidth),
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

            RenderTextInBoxWithOffset(sceneText, width, consoleLine, sb, consoleCharacter, heightOffset: 0, widthOffset: 0,
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
            RenderTextInBoxWithOffset(logs, width, consoleLine, sb, consoleCharacter, heightOffset, widthOffset,
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
            RenderTextInBoxWithOffset(overviewText, width, consoleLine, sb, consoleCharacter, heightOffset, widthOffset,
                height - heightCutOff - 2, widthCutoff, BorderType.SolidBorder, headerText: "   OVERVIEW");
            return true;
        }

        return false;
    }

    private static void RenderTextInBoxWithOffset(
        string[] bodyText,
        int width,
        int consoleLine,
        StringBuilder sb,
        int consoleCharacter,
        int heightOffset,
        int widthOffset,
        int heightCutoff,
        int widthCutoff,
        BorderType borderType = BorderType.NoBorder,
        string? headerText = default,
        string? footerText = default,
        int textTopMargin = 0,
        int textLeftMargin = 0,
        int bodyTextScrollHeight = 0,
        TextLayoutType textLayout = TextLayoutType.TopLeft)
    {
        int localizedLine = consoleLine - heightOffset;
        int localizedCharacter = consoleCharacter - widthOffset;

        // box outline
        if (borderType != BorderType.NoBorder)
        {
            if (localizedCharacter is 0 && localizedLine is 0)
            {
                sb.Append('╔');
                return;
            }
            if (localizedCharacter is 0 && localizedLine == heightCutoff)
            {
                sb.Append('╚');
                return;
            }
            if (localizedCharacter == widthCutoff - 1 && localizedLine is 0)
            {
                sb.Append('╗');
                return;
            }
            if (localizedCharacter == widthCutoff - 1 && localizedLine == heightCutoff)
            {
                sb.Append('╝');
                return;
            }
            if (localizedCharacter is 0 || localizedCharacter == widthCutoff - 1)
            {
                sb.Append('║');
                return;
            }
            if (localizedLine is 0)
            {
                if (headerText != null &&
                    localizedCharacter < (headerText.Length + 1) &&
                    !char.IsWhiteSpace(headerText[localizedCharacter - 1]))
                {
                    sb.Append(headerText[localizedCharacter - 1]);
                }
                else
                {
                    sb.Append('═');
                }
                return;
            }
            if (localizedLine == heightCutoff)
            {
                sb.Append('═');
                return;
            }

            localizedLine = consoleLine - heightOffset - 1;
            localizedCharacter = consoleCharacter - widthOffset - 1;
        }

        int characterOffset = 0;
        if (textLayout == TextLayoutType.TopCenter &&
            localizedLine >= textTopMargin &&
            (localizedLine - textTopMargin + bodyTextScrollHeight) < bodyText.Length)
        {
            string bodyTextLine = bodyText[localizedLine - textTopMargin + bodyTextScrollHeight];
            characterOffset = (widthCutoff / 2) - (bodyTextLine.Length / 2);
            characterOffset = Math.Max(characterOffset, 0);
        }

        if (localizedLine == heightCutoff - 2 &&
            footerText is not null &&
            localizedCharacter >= textLeftMargin)
        {
            if ((localizedCharacter - textLeftMargin) < footerText.Length)
            {
                char ch = footerText[localizedCharacter - textLeftMargin];
                sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);
            }
            else
            {
                sb.Append(' ');
            }

        }
        else if (consoleCharacter < width - 1 &&
            (localizedCharacter - characterOffset) >= textLeftMargin &&
            localizedLine >= textTopMargin &&
            (localizedLine - textTopMargin + bodyTextScrollHeight) < bodyText.Length &&
            (localizedCharacter - characterOffset - textLeftMargin) < bodyText[localizedLine - textTopMargin + bodyTextScrollHeight].Length)
        {
            char ch = bodyText[localizedLine - textTopMargin + bodyTextScrollHeight][localizedCharacter - characterOffset - textLeftMargin];
            sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);
        }
        else
        {
            sb.Append(' ');
        }
    }
}
