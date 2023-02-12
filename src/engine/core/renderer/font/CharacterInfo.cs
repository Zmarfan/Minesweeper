using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.renderer.font; 

public class CharacterInfo {
    public readonly char character;
    public Vector2Int dimension;
    public readonly Dictionary<char, int> kerningByCharacter = new();
    public readonly SDL.SDL_FPoint[] textureCoords;

    public CharacterInfo(char character, SDL.SDL_Rect rect) {
        this.character = character;
        dimension = new Vector2Int(rect.w, rect.h);
        Vector2 topLeft = new(
            Math.Clamp(rect.x / (float)Font.ATLAS_SIZE, 0, 1), 
            Math.Clamp(rect.y / (float)Font.ATLAS_SIZE, 0, 1)
        );
        Vector2 bottomRight = new(
            Math.Clamp((rect.x + rect.w) / (float)Font.ATLAS_SIZE, 0, 1), 
            Math.Clamp((rect.y + rect.h) / (float)Font.ATLAS_SIZE, 0, 1)
        );
        textureCoords = new[] {
            new SDL.SDL_FPoint { x = topLeft.x, y = topLeft.y },
            new SDL.SDL_FPoint { x = bottomRight.x, y = topLeft.y },
            new SDL.SDL_FPoint { x = topLeft.x, y = bottomRight.y },
            new SDL.SDL_FPoint { x = bottomRight.x, y = bottomRight.y }
        };
    }
}