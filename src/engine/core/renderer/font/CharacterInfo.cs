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
        textureCoords = new[] {
            new SDL.SDL_FPoint { x = rect.x / (float)Font.ATLAS_SIZE, y = rect.y / (float)Font.ATLAS_SIZE },
            new SDL.SDL_FPoint { x = (rect.x + rect.w) / (float)Font.ATLAS_SIZE, y = rect.y / (float)Font.ATLAS_SIZE },
            new SDL.SDL_FPoint { x = rect.x / (float)Font.ATLAS_SIZE, y = (rect.y + rect.h) / (float)Font.ATLAS_SIZE },
            new SDL.SDL_FPoint { x = (rect.x + rect.w) / (float)Font.ATLAS_SIZE, y = (rect.y + rect.h) / (float)Font.ATLAS_SIZE }
        };
    }
}