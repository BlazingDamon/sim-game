using System.Text;

namespace Main;
internal class GameRender
{
    public static void RenderWorldMapView(string[] mapText)
    {
        Console.CursorVisible = false;

        var (width, height) = ConsoleUtils.GetWidthAndHeight();
        int heightCutOff = (int)(height * .80);
        int widthCutoff = (int)(width * .50);

        StringBuilder sb = new(width * height);
        for (int j = 0; j < height; j++)
        {
            if (OperatingSystem.IsWindows() && j == height - 1)
            {
                break;
            }

            for (int i = 0; i < width; i++)
            {
                // "helper text" area
                if (j >= heightCutOff && i < widthCutoff)
                {
                    int line = j - heightCutOff;
                    int character = i - 1;
                    if (i < width - 1 && character >= 0 && line >= 0 && line < mapText.Length && character < mapText[line].Length)
                    {
                        char ch = mapText[line][character];
                        sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                    continue;
                }

                // debug log area
                if (j >= heightCutOff && i >= widthCutoff)
                {
                    string[] debugLogHeader = ["DEBUG LOGS"];
                    string[] debugLogs = GameDebugLogger.ReadLogs(Math.Max(height - heightCutOff - 2, 0));
                    string[] fullTextSection = ArrayUtils.ConcatArrays(debugLogHeader, debugLogs);

                    int line = j - heightCutOff;
                    int character = i - widthCutoff - 1;
                    if (i < width - 1 && character >= 0 && line >= 0 && line < fullTextSection.Length && character < fullTextSection[line].Length)
                    {
                        char ch = fullTextSection[line][character];
                        sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                    continue;
                }

                // main area
                if (j < heightCutOff - 1 && i >= 1 && j >= 1 && i < width - 1)
                {
                    string[] summaryView = GameBaker.BakedSummaryView;
                    int line = j - 1;
                    int character = i - 1;
                    if (i < width - 1 && character >= 0 && line >= 0 && line < summaryView.Length && character < summaryView[line].Length)
                    {
                        char ch = summaryView[line][character];
                        sb.Append(char.IsWhiteSpace(ch) ? ' ' : ch);
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                    continue;
                }

                // map outline
                if (i is 0 && j is 0)
                {
                    sb.Append('╔');
                    continue;
                }
                if (i is 0 && j == heightCutOff - 1)
                {
                    sb.Append('╚');
                    continue;
                }
                if (i == width - 1 && j is 0)
                {
                    sb.Append('╗');
                    continue;
                }
                if (i == width - 1 && j == heightCutOff - 1)
                {
                    sb.Append('╝');
                    continue;
                }
                if (i is 0 || i == width - 1)
                {
                    sb.Append('║');
                    continue;
                }
                if (j is 0 || j == heightCutOff - 1)
                {
                    sb.Append('═');
                    continue;
                }

                // tiles
                char c = ' ';
                sb.Append(char.IsWhiteSpace(c) ? ' ' : c);
            }
            if (!OperatingSystem.IsWindows() && j < height - 1)
            {
                sb.AppendLine();
            }
        }
        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
    }
}
