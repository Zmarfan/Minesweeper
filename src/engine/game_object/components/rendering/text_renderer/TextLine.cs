namespace Worms.engine.game_object.components.rendering.text_renderer; 

public class TextLine {
    public readonly string text;
    public readonly float fraction;

    public TextLine(string text, float lineWidth, float maxWidth) {
        this.text = text;
        fraction = Math.Clamp(1 - lineWidth / maxWidth, 0, 1);
    }
}