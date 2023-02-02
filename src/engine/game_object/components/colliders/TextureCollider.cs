using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.colliders; 

public class TextureCollider : Script {
    private readonly TextureRenderer _textureRenderer;
    private readonly PixelCollider _pixelCollider;
    
    public TextureCollider(bool isActive, bool isTrigger, bool edgesOnly, TextureRendererBuilder builder) : base(isActive) {
        IsActive = isActive;
        _textureRenderer = builder.SetIsActive(true).Build();
        _pixelCollider = new PixelCollider(true, CalculateColliderPixels(edgesOnly), isTrigger, Vector2.Zero());
    }

    public override void Awake() {
        AddComponent(_textureRenderer);
        AddComponent(_pixelCollider);
    }

    private IEnumerable<Vector2> CalculateColliderPixels(bool edgesOnly) {
        Color[,] colors = _textureRenderer.texture.pixels;
        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        int evenWidthOffset = (width + 1) % 2;
        int evenHeightOffset = (height + 1) % 2;
        HashSet<Vector2> pixels = new();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (ShouldSetPixelAsCollider(colors, x, y, edgesOnly)) {
                    pixels.Add(new Vector2(x - width / 2 + evenWidthOffset, -y + height / 2 - evenHeightOffset));
                } 
            }
        }

        return pixels;
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

    private static bool IsAnEdgePixel(int x, int y, Color[,] colors) {
        return x == 0 || y == 0 || x == colors.GetLength(0) - 1 || y == colors.GetLength(1) - 1;
    }
}