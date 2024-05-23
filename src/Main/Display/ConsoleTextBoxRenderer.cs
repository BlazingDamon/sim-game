using Main.Menus.Base;
using System.Text;

namespace Main.Display;
internal class ConsoleTextBoxRenderer
{
    public static void RenderTextBox(
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
