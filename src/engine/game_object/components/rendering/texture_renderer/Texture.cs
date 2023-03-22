using SDL2;
using GameEngine.engine.core.renderer.textures;
using GameEngine.engine.data;

namespace GameEngine.engine.game_object.components.rendering.texture_renderer; 

public struct Texture {
    public Color[,] SectionPixels => _sectionPixels ??= CalculateSectionPixels();
    public string textureId;
    public readonly Vector2 textureScale;

    public int Width => SectionPixels.GetLength(0);
    public int Height => SectionPixels.GetLength(1);
    
    private readonly int _column;
    private readonly int _row;
    private readonly int _columnLength;
    private readonly int _rowLength;

    public Color[,] texturePixels;
    private Color[,]? _sectionPixels = null;
    
    private Texture(string textureId, int column, int row, int columnLength, int rowLength) {
        this.textureId = textureId;
        textureScale = new Vector2(1f / columnLength, 1f / rowLength);
        _column = column;
        _row = row;
        _columnLength = columnLength;
        _rowLength = rowLength;
        StoredTexture storedTexture = TextureStorage.GetStoredTexture(textureId);
        texturePixels = storedTexture.pixels;
    }
    
    private Texture(Color[,] pixels, int column, int row, int columnLength, int rowLength) {
        textureScale = new Vector2(1f / columnLength, 1f / rowLength);
        _column = column;
        _row = row;
        _columnLength = columnLength;
        _rowLength = rowLength;
        TextureStorage.LoadTextureFromPixels(pixels, out textureId);
        texturePixels = pixels;
    }


    public static Texture CreateSingle(string textureId) {
        return new Texture(textureId, 0, 0, 1, 1);
    }

    public static Texture CreateSingle(Color[,] pixels) {
        return new Texture(pixels, 0, 0, 1, 1);
    }
    
    public static Texture CreateMultiple(string textureSrc, int column, int row, int columnLength, int rowLength) {
        return new Texture(textureSrc, column, row, columnLength, rowLength);
    }
    
    public static Texture CreateMultiple(Color[,] pixels, int column, int row, int columnLength, int rowLength) {
        return new Texture(pixels, column, row, columnLength, rowLength);
    }

    public void Alter(Color[,] pixels) {
        _sectionPixels = null;
        TextureStorage.AlterTexture(textureId, texturePixels, pixels, out textureId, out StoredTexture storedTexture);
        texturePixels = storedTexture.pixels;
    }

    internal unsafe SDL.SDL_Rect GetSrcRect(StoredTexture storedTexture) {
        int w = storedTexture.surface->w / _columnLength;
        int h = storedTexture.surface->h / _rowLength;
        return new SDL.SDL_Rect { x = _column * w, y = _row * h, w = w, h = h };
    }

    private Color[,] CalculateSectionPixels() {
        if (_columnLength == 1 && _rowLength == 1) {
            return texturePixels;
        }
        
        int width = texturePixels.GetLength(0) / _columnLength;
        int height = texturePixels.GetLength(1) / _rowLength;
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