using System.Text;
using Worms.engine.core.renderer.font;

namespace Worms.engine.game_object.components.rendering.text_renderer; 

public static class TextFormatter {
    public static List<TextLine> FormatText(string text, int width, int size, Font font) {
        float sizeModifier = size / (float)Font.FONT_SIZE;
        List<TextLine> allLines = new();
        
        StringBuilder line = new();
        float lineWidth = 0;
        StringBuilder word = new();
        float wordWidth = 0;
        char? previousWordCharacter = null;
        
        for (int i = 0; i < text.Length; i++) {
            char c = font.characters.ContainsKey(text[i]) || text[i] == '\n' ? text[i] : '?';
            
            if (c == '\n') {
                if (lineWidth + wordWidth >= width) {
                    allLines.Add(new TextLine(line.ToString(), lineWidth, width));
                    line.Clear();
                    lineWidth = 0;
                }
                else if (lineWidth != 0) {
                    line.Append(' ');
                    lineWidth += font.characters[' '].dimension.x * sizeModifier;
                }

                lineWidth += wordWidth;
                line.Append(word);
                
                allLines.Add(new TextLine(line.ToString(), lineWidth, width));

                wordWidth = 0;
                lineWidth = 0;
                line.Clear();
                word.Clear();
                previousWordCharacter = null;
                continue;
            }

            CharacterInfo info = font.characters[c];
            int kerning = !previousWordCharacter.HasValue  ? 0 : info.kerningByCharacter[previousWordCharacter.Value];
            float charWidth = (info.dimension.x + kerning) * sizeModifier;

            if (wordWidth >= width) {
                if (lineWidth != 0) {
                    allLines.Add(new TextLine(line.ToString(), lineWidth, width));
                    lineWidth = 0;
                    line.Clear();
                }
                
                allLines.Add(new TextLine(word.ToString(), wordWidth, width));

                wordWidth = charWidth;
                word.Clear();
                word.Append(c);
                previousWordCharacter = c;
                continue;
            }

            if (c != ' ') {
                wordWidth += charWidth;
                word.Append(c);
                previousWordCharacter = c;
            }
            
            if (c == ' ' || i == text.Length - 1) {
                if (lineWidth + wordWidth >= width) {
                    allLines.Add(new TextLine(line.ToString(), lineWidth, width));
                    lineWidth = 0;
                    line.Clear();
                }
                else if (lineWidth != 0) {
                    line.Append(' ');
                    lineWidth += font.characters[' '].dimension.x * sizeModifier;
                }

                line.Append(word);
                lineWidth += wordWidth;
                wordWidth = 0;
                word.Clear();
                previousWordCharacter = null;
            }
        }

        allLines.Add(new TextLine(line.ToString(), lineWidth, width));
        return allLines;
    }
}