using SDL2;

namespace Worms.engine.core.renderer.font; 

public class CharacterInfo {
    public readonly char character;
    public readonly SDL.SDL_Rect rect;
    public readonly Dictionary<char, int> kerningByCharacter = new();

    public CharacterInfo(char character, SDL.SDL_Rect rect) {
        this.character = character;
        this.rect = rect;
    }
}