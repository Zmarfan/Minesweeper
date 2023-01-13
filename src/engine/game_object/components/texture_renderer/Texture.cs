using SDL2;
using Worms.engine.core.renderer;

namespace Worms.engine.game_object.components.texture_renderer; 

public readonly struct Texture {
    private static readonly string ROOT_DIRECTORY = Directory.GetCurrentDirectory();
    
    public readonly string textureSrc;
    private readonly int _column;
    private readonly int _row;
    private readonly int _columnLength;
    private readonly int _rowLength;

    private Texture(string textureSrc, int column, int row, int columnLength, int rowLength) {
        this.textureSrc = $"{ROOT_DIRECTORY}\\{textureSrc}";
        _column = column;
        _row = row;
        _columnLength = columnLength;
        _rowLength = rowLength;
    }

    public static Texture CreateSingle(string textureSrc) {
        return new Texture(textureSrc, 0, 0, 1, 1);
    }

    public static Texture CreateMultiple(string textureSrc, int column, int row, int columnLength, int rowLength) {
        return new Texture(textureSrc, column, row, columnLength, rowLength);
    }

    public unsafe SDL.SDL_Rect GetSrcRect(StoredTexture storedTexture) {
        int w = storedTexture.surface->w / _columnLength;
        int h = storedTexture.surface->h / _rowLength;
        return new SDL.SDL_Rect { x = _column * w, y = _row * h, w = w, h = h };
    }

    public override string ToString() {
        return $"Src: {textureSrc}, column: {_column} / {_columnLength}, row: {_row} / {_rowLength}";
    }
}