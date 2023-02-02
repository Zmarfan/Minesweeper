using SDL2;
using Worms.engine.core.renderer;
using Worms.engine.data;

namespace Worms.engine.game_object.components.texture_renderer; 

public readonly struct Texture {
    public readonly string textureId;
    public readonly Vector2 textureScale;
    public readonly unsafe SDL.SDL_Surface* surface;
    public readonly Color[,] texturePixels;
    public readonly Color[,] sectionPixels;
    private readonly int _column;
    private readonly int _row;
    private readonly int _columnLength;
    private readonly int _rowLength;

    
    private unsafe Texture(string textureSrc, int column, int row, int columnLength, int rowLength) {
        textureId = textureSrc;
        textureScale = new Vector2(1f / columnLength, 1f / rowLength);
        _column = column;
        _row = row;
        _columnLength = columnLength;
        _rowLength = rowLength;
        TextureRendererHandler.LoadImage(textureId, textureSrc, out SDL.SDL_Surface* surfaceData, out Color[,] pixelData);
        surface = surfaceData;
        texturePixels = pixelData;
        sectionPixels = CalculateSectionPixels();
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

    private unsafe Color[,] CalculateSectionPixels() {
        if (_columnLength == 1 && _rowLength == 1) {
            return texturePixels;
        }
        
        int width = surface->w / _columnLength;
        int height = surface->h / _rowLength;
        int startWidth = width * _column;
        int startHeight = height * _row;

        Color[,] pixels = new Color[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                pixels[x, y] = texturePixels[startWidth + x, startHeight + y];
            }
        }

        return pixels;
    }
    
    public override string ToString() {
        return $"Src: {textureId}, column: {_column} / {_columnLength}, row: {_row} / {_rowLength}";
    }
}