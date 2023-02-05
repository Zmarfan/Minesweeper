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
            _pixelCollider.pixels = value.SectionPixels;
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
            _textureRenderer.flipX = value;
            _pixelCollider.flipX = value;
        }
    }

    public bool FlipY {
        get => _textureRenderer.flipY;
        set {
            _textureRenderer.flipY = value;
            _pixelCollider.flipY = value;
        }
    }

    private readonly TextureRenderer _textureRenderer;
    private readonly PixelCollider _pixelCollider;
    private Random _random = new();
    
    public TextureCollider(bool isActive, bool isTrigger, TextureRenderer tr) : base(isActive) {
        IsActive = isActive;
        _textureRenderer = tr;
        _pixelCollider = new PixelCollider(true, tr.texture.SectionPixels, tr.flipX, tr.flipY, isTrigger, Vector2Int.Zero());
    }

    public override void Awake() {
        AddComponent(_textureRenderer);
        AddComponent(_pixelCollider);
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown("alterTexture")) {
            int width = _textureRenderer.texture.SectionPixels.GetLength(0);
            int height = _textureRenderer.texture.SectionPixels.GetLength(1);
            int randomX = _random.Next(0, width);
            int randomY = _random.Next(0, height);
            Color[,] newPixels = (Color[,])_textureRenderer.texture.SectionPixels.Clone();
            for (int x = Math.Max(randomX - 50, 0); x < Math.Min(randomX + 50, width); x++) {
                for (int y = Math.Max(randomY - 50, 0); y < Math.Min(randomY + 50, height); y++) {
                    newPixels[x, y] = Color.TRANSPARENT;
                }
            }
            
            Texture texture = Texture;
            texture.Alter(newPixels);
            Texture = texture;
        }
    }
}