﻿using Main.Menus.Base;
using System.Text;
using System.Text.RegularExpressions;

namespace Main.Display;
internal class ConsoleRenderer
{
    public static void Render()
    {
        Console.CursorVisible = false;
        var sceneText = GameGlobals.CurrentGameState.CurrentScene.SceneText;
        var overviewText = GameBaker.BakedOverview;
        var currentMenuExists = GameGlobals.MenuStack.TryPeek(out Menu? menu);

        var (width, height) = ConsoleUtils.GetWidthAndHeight();
        int heightCutoff = Math.Min(height - 8, (int)(height * .80));
        int widthCutoff = Math.Min(40, (int)(width * .30));

        StringBuilder sb = new(width * height);
        
        for (int j = 0; j < height; j++)
        {
            if (OperatingSystem.IsWindows() && j == height - 1)
            {
                break;
            }

            if (currentMenuExists)
            {
                if (menu!.Layout == LayoutType.FullScreen)
                {
                    sb.Append(RenderOneLineOfFullScreenMenu(menu, width, height, j));
                }
                else if (menu!.Layout == LayoutType.RightFixed || menu!.Layout == LayoutType.RightThird)
                {
                    sb.Append(RenderOneLineOfRightScreenMenuLayout(menu, overviewText, sceneText, width, height, heightCutoff, widthCutoff, j));
                }
            }
            else
            {
                sb.Append(RenderOneLineOfDefaultLayout(overviewText, sceneText, width, height, heightCutoff, widthCutoff, j));
            }

            if (!OperatingSystem.IsWindows() && j < height - 1)
            {
                sb.AppendLine();
            }

        }

        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
    }

    private static string RenderOneLineOfFullScreenMenu(Menu menu, int width, int height, int j)
    {
        StringBuilder sb = new(width);

        for (int i = 0; i < width; i++)
        {
            RenderTextInBoxWithOffset(menu.MenuBody, width, j, sb, i, 0, 0, height - 2, width, BorderType.SolidBorder, menu.MenuTitle, textTopMargin: 1, textLeftMargin: 2, textLayout: menu.TextLayout);
        }

        return sb.ToString();
    }


    private static string RenderOneLineOfRightScreenMenuLayout(Menu menu, string[] overviewText, string[] sceneText, int width, int height, int heightCutOff, int widthCutoff, int j)
    {
        StringBuilder sb = new(width);
        int mainDisplayScroll = Math.Clamp(GameGlobals.MainDisplayScrollHeight, 0, Math.Max(0, sceneText.Length - heightCutOff + 3));
        GameGlobals.MainDisplayScrollHeight = mainDisplayScroll;

        for (int i = 0; i < width; i++)
        {
            // "summary text" area
            if (RenderOverviewCorner(overviewText, width, height, heightCutOff, widthCutoff, j, sb, i))
                continue;

            // debug log area
            if (RenderLogsCorner(width, height, heightCutOff, widthCutoff, j, sb, i))
                continue;

            int menuWidth;
            if (menu.Layout == LayoutType.RightThird)
                menuWidth = (int)(width * 0.3333);
            else if (menu.Layout == LayoutType.RightFixed && menu.MenuWidth.HasValue)
                menuWidth = width - menu.MenuWidth.Value;
            else
                throw new Exception("There is a misconfigured menu layout. Check that MenuWidth is set correctly, or that your LayoutType is handled correctly.");

            // main area
            if (j < heightCutOff && i >= 0 && j >= 0 && i < (width - menuWidth))
            {
                var heightOffset = 0;
                var widthOffset = 0;
                RenderTextInBoxWithOffset(sceneText, width, j, sb, i, heightOffset, widthOffset, heightCutOff - 1, (width - menuWidth), BorderType.SolidBorder, $"{(GameGlobals.IsSimulationRunning ? "" : "   PAUSED")}", footerText: (sceneText.Length > heightCutOff - 1 ? "[j] to scroll down, [k] to scroll up" : null), bodyTextScrollHeight: mainDisplayScroll);
                continue;
            }

            // menu area
            if (j < heightCutOff && i >= 0 && j >= 0 && i >= (width - menuWidth))
            {
                RenderTextInBoxWithOffset(menu.MenuBody, width, j, sb, i, 0, (width - menuWidth), heightCutOff - 1, menuWidth, BorderType.SolidBorder, headerText: menu.MenuTitle, footerText: menu.MenuFooter, textTopMargin: 1, textLeftMargin: 2, textLayout: menu.TextLayout);
                continue;
            }

            // if you made it here, this is just extra whitespace to fill out the lines
            char c = ' ';
            sb.Append(char.IsWhiteSpace(c) ? ' ' : c);
        }

        return sb.ToString();
    }

    private static bool RenderLogsCorner(int width, int height, int heightCutOff, int widthCutoff, int j, StringBuilder sb, int i)
    {
        if (j >= heightCutOff && i >= widthCutoff)
        {
            string[] logs;
            if (GameGlobals.IsDebugModeEnabled)
                logs = GameDebugLogger.ReadLogs(Math.Max(height - heightCutOff - 3, 0));
            else
                logs = GameGlobals.CurrentGameState.GameLogger.ReadLogs(Math.Max(height - heightCutOff - 3, 0));

            var heightOffset = heightCutOff;
            var widthOffset = widthCutoff;
            RenderTextInBoxWithOffset(logs, width, j, sb, i, heightOffset, widthOffset, height - heightCutOff - 2, width - widthCutoff, BorderType.SolidBorder, headerText: "   GAME LOGS");
            return true;
        }

        return false;
    }

    private static bool RenderOverviewCorner(string[] overviewText, int width, int height, int heightCutOff, int widthCutoff, int j, StringBuilder sb, int i)
    {
        if (j >= heightCutOff && i < widthCutoff)
        {
            var heightOffset = heightCutOff;
            var widthOffset = 0;
            RenderTextInBoxWithOffset(overviewText, width, j, sb, i, heightOffset, widthOffset, height - heightCutOff - 2, widthCutoff, BorderType.SolidBorder, headerText: "   OVERVIEW");
            return true;
        }

        return false;
    }

    private static string RenderOneLineOfDefaultLayout(string[] overviewText, string[] sceneText, int width, int height, int heightCutOff, int widthCutoff, int j)
    {
        StringBuilder sb = new(width);
        int mainDisplayScroll = Math.Clamp(GameGlobals.MainDisplayScrollHeight, 0, Math.Max(0, sceneText.Length - heightCutOff + 3));
        GameGlobals.MainDisplayScrollHeight = mainDisplayScroll;

        for (int i = 0; i < width; i++)
        {
            if (RenderOverviewCorner(overviewText, width, height, heightCutOff, widthCutoff, j, sb, i))
                continue;

            if (RenderLogsCorner(width, height, heightCutOff, widthCutoff, j, sb, i))
                continue;

            // main area
            if (j < heightCutOff && i >= 0 && j >= 0 && i < width)
            {
                var heightOffset = 0;
                var widthOffset = 0;
                RenderTextInBoxWithOffset(sceneText, width, j, sb, i, heightOffset, widthOffset, heightCutOff - 1, width, BorderType.SolidBorder, $"{(GameGlobals.IsSimulationRunning ? "" : "   PAUSED")}", footerText: (sceneText.Length > heightCutOff - 1 ? "[j] to scroll down, [k] to scroll up" : null), bodyTextScrollHeight: mainDisplayScroll);
                continue;
            }

            // if you made it here, this is just extra whitespace to fill out the lines
            char c = ' ';
            sb.Append(char.IsWhiteSpace(c) ? ' ' : c);
        }

        return sb.ToString();
    }

    private static void RenderTextInBoxWithOffset(
        string[] bodyText,
        int width,
        int j,
        StringBuilder sb,
        int i,
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
        int line = j - heightOffset;
        int character = i - widthOffset;

        // box outline
        if (borderType != BorderType.NoBorder)
        {
            if (character is 0 && line is 0)
            {
                sb.Append('╔');
                return;
            }
            if (character is 0 && line == heightCutoff)
            {
                sb.Append('╚');
                return;
            }
            if (character == widthCutoff - 1 && line is 0)
            {
                sb.Append('╗');
                return;
            }
            if (character == widthCutoff - 1 && line == heightCutoff)
            {
                sb.Append('╝');
                return;
            }
            if (character is 0 || character == widthCutoff - 1)
            {
                sb.Append('║');
                return;
            }
            if (line is 0)
            {
                if (headerText != null && character < (headerText.Length + 1) && !char.IsWhiteSpace(headerText[character - 1]))
                {
                    sb.Append(headerText[character - 1]);
                }
                else
                {
                    sb.Append('═');
                }
                return;
            }
            if (line == heightCutoff)
            {
                sb.Append('═');
                return;
            }

            line = j - heightOffset - 1;
            character = i - widthOffset - 1;
        }

        int characterOffset = 0;
        if (textLayout == TextLayoutType.TopCenter && line >= textTopMargin && (line - textTopMargin + bodyTextScrollHeight) < bodyText.Length)
        {
            string bodyTextLine = bodyText[line - textTopMargin + bodyTextScrollHeight];
            characterOffset = (widthCutoff / 2) - (bodyTextLine.Length / 2);
            characterOffset = Math.Max(characterOffset, 0);
        }

        if (line == heightCutoff - 2 && footerText is not null && character >= textLeftMargin && (character - textLeftMargin) < footerText.Length)
        {
            char ch = footerText[character - textLeftMargin];
            sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);

        }
        else if (i < width - 1 && (character - characterOffset) >= textLeftMargin && line >= textTopMargin && (line - textTopMargin + bodyTextScrollHeight) < bodyText.Length && (character - characterOffset - textLeftMargin) < bodyText[line - textTopMargin + bodyTextScrollHeight].Length)
        {
            char ch = bodyText[line - textTopMargin + bodyTextScrollHeight][character - characterOffset - textLeftMargin];
            sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);
        }
        else
        {
            sb.Append(' ');
        }
    }
}
