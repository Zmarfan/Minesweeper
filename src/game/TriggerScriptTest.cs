using Worms.engine.data;
using Worms.engine.game_object.components.colliders;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class TriggerScriptTest : Script {
    private TextureRenderer _textureRenderer = null!;
    
    public TriggerScriptTest() : base(true) {
    }

    public override void Start() {
        _textureRenderer = GetComponent<TextureRenderer>();
    }

    public override void OnMouseEnter() {
        _textureRenderer.color = Color.TRANSPARENT;
    }

    public override void OnMouseExit() {
        _textureRenderer.color = Color.WHITE;
    }
}