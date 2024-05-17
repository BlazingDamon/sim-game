﻿using Main.Menus.Base;
using System.Text;

namespace Main.Display;
internal class ConsoleRenderer
{
    public static void Render()
    {
        Console.CursorVisible = false;
        var mapText = GameGlobals.CurrentGameState.CurrentScene.MapText;
        var currentMenuExists = GameGlobals.MenuStack.TryPeek(out Menu? menu);

        var (width, height) = ConsoleUtils.GetWidthAndHeight();
        int heightCutOff = (int)(height * .80);
        int widthCutoff = Math.Min(40, (int)(width * .30));

        StringBuilder sb = new(width * height);
        for (int j = 0; j < height; j++)
        {
            if (OperatingSystem.IsWindows() && j == height - 1)
            {
                break;
            }

            if (currentMenuExists && menu!.Layout == LayoutType.FullScreen)
            {
                sb.Append(RenderOneLineOfFullScreenMenu(menu.MenuBody, menu.MenuTitle, width, height, j));
            }
            else
            {
                sb.Append(RenderOneLineOfDefaultView(mapText, width, height, heightCutOff, widthCutoff, j));
            }

            if (!OperatingSystem.IsWindows() && j < height - 1)
            {
                sb.AppendLine();
            }
        }
        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
    }
    private static string RenderOneLineOfFullScreenMenu(string[] bodyText, string headerText, int width, int height, int j)
    {
        StringBuilder sb = new(width);

        for (int i = 0; i < width; i++)
        {
            RenderTextInBoxWithOffset(bodyText, width, j, sb, i, 0, 0, height - 2, width, BorderType.SolidBorder, headerText, textTopMargin: 1, textLeftMargin: 2);
        }

        return sb.ToString();
    }
    private static string RenderOneLineOfDefaultView(string[] mapText, int width, int height, int heightCutOff, int widthCutoff, int j)
    {
        StringBuilder sb = new(width);

        for (int i = 0; i < width; i++)
        {
            // "menu text" area
            if (j >= heightCutOff && i < widthCutoff)
            {
                var heightOffset = heightCutOff;
                var widthOffset = 0;
                RenderTextInBoxWithOffset(mapText, width, j, sb, i, heightOffset, widthOffset, height - heightCutOff - 2, widthCutoff, BorderType.SolidBorder, "   MENU OPTIONS");
                continue;
            }

            // debug log area
            if (j >= heightCutOff && i >= widthCutoff)
            {
                string[] debugLogs = GameDebugLogger.ReadLogs(Math.Max(height - heightCutOff - 3, 0));

                var heightOffset = heightCutOff;
                var widthOffset = widthCutoff;
                RenderTextInBoxWithOffset(debugLogs, width, j, sb, i, heightOffset, widthOffset, height - heightCutOff - 2, width - widthCutoff, BorderType.SolidBorder, "   DEBUG LOGS");
                continue;
            }

            // main area
            if (j < heightCutOff && i >= 0 && j >= 0 && i < width)
            {
                string[] summaryView = GameBaker.BakedSummaryView;

                var heightOffset = 0;
                var widthOffset = 0;
                RenderTextInBoxWithOffset(summaryView, width, j, sb, i, heightOffset, widthOffset, heightCutOff - 1, width, BorderType.SolidBorder, $"{(GameGlobals.IsSimulationRunning ? "" : "   PAUSED")}");
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
        int textTopMargin = 0,
        int textLeftMargin = 0)
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

        if (i < width - 1 && character >= textLeftMargin && line >= textTopMargin && (line - textTopMargin) < bodyText.Length && (character - textLeftMargin) < bodyText[line - textTopMargin].Length)
        {
            char ch = bodyText[line - textTopMargin][character - textLeftMargin];
            sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);
        }
        else
        {
            sb.Append(' ');
        }
    }
}
