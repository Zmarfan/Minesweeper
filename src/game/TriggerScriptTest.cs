using Worms.engine.data;
using Worms.engine.game_object.components.colliders;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class TriggerScriptTest : Script {
    private BoxCollider _collider = null!;
    private TextureRenderer _textureRenderer = null!;
    private Vector2 _defaultSize;
    
    public TriggerScriptTest() : base(true) {
    }

    public override void Awake() {
        _collider = GetComponent<BoxCollider>();
        _textureRenderer = GetComponent<TextureRenderer>();
        _defaultSize = _collider.size;
    }

    public override void OnMouseEnter() {
        _collider.size = _defaultSize * 1.5f;
        _textureRenderer.color = Color.RED;
    }

    public override void OnMouseExit() {
        _collider.size = _defaultSize;
        _textureRenderer.color = Color.WHITE;
    }
}