using System.Diagnostics;
using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.colliders; 

public class TextureCollider : Script {
    public Texture Texture {
        get => _textureRenderer.texture;
        set {
            _textureRenderer.texture = value;
            Stopwatch stopwatch = new();
            stopwatch.Start();
            _pixelCollider.Pixels = CalculateColliderPixels(EdgesOnly).ToHashSet();
            stopwatch.Stop();
            Console.WriteLine($"collider: {stopwatch.ElapsedMilliseconds / 1000f}");
        }
    }
    public string SortingLayer {
        get => _textureRenderer.sortingLayer;
        set => _textureRenderer.sortingLayer = value;
    }
    public int OrderInLayer {
        get => _textureRenderer.orderInLayer;
        set => _textureRenderer.orderInLayer = value;
    }
    public Color Color {
        get => _textureRenderer.color;
        set => _textureRenderer.color = value;
    }
    public bool FlipX {
        get => _textureRenderer.flipX;
        set {
            if (_textureRenderer.flipX == value) {
                return;
            }

            _pixelCollider.Pixels = _pixelCollider.Pixels.Select(p => {
                p.x = -p.x + EvenWidthOffset;
                return p;
            }).ToHashSet();
            _textureRenderer.flipX = value;
        }
    }

    public bool FlipY {
        get => _textureRenderer.flipY;
        set {
            if (_textureRenderer.flipY == value) {
                return;
            }

            _pixelCollider.Pixels = _pixelCollider.Pixels.Select(p => {
                p.y = -p.y - EvenWidthOffset;
                return p;
            }).ToHashSet();
            _textureRenderer.flipY = value;
        }
    }

    public bool EdgesOnly {
        get => _edgesOnly;
        set {
            if (value == _edgesOnly) {
                return;
            }
            _edgesOnly = value;
            _pixelCollider.Pixels = CalculateColliderPixels(value).ToHashSet();
        }
    }
    
    private readonly TextureRenderer _textureRenderer;
    private readonly PixelCollider _pixelCollider;
    private bool _edgesOnly;
    
    private int EvenWidthOffset => (_textureRenderer.texture.SectionPixels.GetLength(0) + 1) % 2;
    private int EvenHeightOffset => (_textureRenderer.texture.SectionPixels.GetLength(1) + 1) % 2;
    
    public TextureCollider(bool isActive, bool isTrigger, bool edgesOnly, TextureRenderer tr) : base(isActive) {
        IsActive = isActive;
        _textureRenderer = tr;
        _pixelCollider = new PixelCollider(true, CalculateColliderPixels(edgesOnly), isTrigger, Vector2.Zero());
        _edgesOnly = edgesOnly;
    }

    public override void Awake() {
        AddComponent(_textureRenderer);
        AddComponent(_pixelCollider);
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown("alterTexture")) {
            Color[,] newPixels = (Color[,])_textureRenderer.texture.SectionPixels.Clone();
            for (int x = 100; x < 200; x++) {
                for (int y = 100; y < 200; y++) {
                    newPixels[x, y] = Color.TRANSPARENT;
                }
            }

            Stopwatch stopwatch = new();
            stopwatch.Start();
            Texture texture = Texture;
            texture.Alter(newPixels);
            Texture = texture;
            stopwatch.Stop();
            Console.WriteLine($"total: {stopwatch.ElapsedMilliseconds / 1000f}");
        }
    }

    private IEnumerable<Vector2> CalculateColliderPixels(bool edgesOnly) {
        Color[,] colors = _textureRenderer.texture.SectionPixels;
        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        HashSet<Vector2> pixels = new();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (ShouldSetPixelAsCollider(colors, x, y, edgesOnly)) {
                    pixels.Add(new Vector2(x - width / 2 + EvenWidthOffset, -y + height / 2 - EvenHeightOffset));
                } 
            }
        }

        return FormatPixels(pixels);
    }

    private bool ShouldSetPixelAsCollider(Color[,] colors, int x, int y, bool edgesOnly) {
        if (!colors[x, y].IsOpaque) {
            return false;
        }

        if (!edgesOnly || IsAnEdgePixel(x, y, colors)) {
            return true;
        }

        for (int neighborX = x - 1; neighborX <= x + 1; neighborX++) {
            for (int neighborY = y - 1; neighborY <= y + 1; neighborY++) {
                if (!colors[neighborX, neighborY].IsOpaque) {
                    return true;
                }
            }
        }

        return false;
    }

    
    private IEnumerable<Vector2> FormatPixels(HashSet<Vector2> pixels) {
        return pixels.Select(p => {
            if (_textureRenderer.flipX) {
                p.x = -p.x + EvenWidthOffset;
            }
            if (_textureRenderer.flipY) {
                p.y = -p.y - EvenWidthOffset;
            }

            return p;
        });
    }
    
    private static bool IsAnEdgePixel(int x, int y, Color[,] colors) {
        return x == 0 || y == 0 || x == colors.GetLength(0) - 1 || y == colors.GetLength(1) - 1;
    }
}