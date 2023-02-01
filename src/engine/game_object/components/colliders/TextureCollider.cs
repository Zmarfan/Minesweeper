using Worms.engine.data;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.colliders; 

public class TextureCollider : Script {
    private readonly TextureRenderer _textureRenderer;
    private readonly PixelCollider _pixelCollider;
    
    public TextureCollider(bool isActive, bool isTrigger, TextureRendererBuilder builder) : base(isActive) {
        IsActive = isActive;
        _textureRenderer = builder.SetIsActive(true).Build();
        _pixelCollider = new PixelCollider(true, CalculateColliderPixels(), isTrigger, Vector2.Zero());
    }

    public override void Awake() {
        AddComponent(_textureRenderer);
        AddComponent(_pixelCollider);
    }

    private IEnumerable<Vector2> CalculateColliderPixels() {
        HashSet<Vector2> pixels = new();

        for (int x = -20; x <= 20; x++) {
            for (int y = -20; y <= 20; y++) {
                pixels.Add(new Vector2(x, y));
            }
        }

        return pixels;
    }
}