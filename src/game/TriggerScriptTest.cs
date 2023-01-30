using Worms.engine.data;
using Worms.engine.game_object.components.colliders;
using Worms.engine.game_object.components.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class TriggerScriptTest : Script {
    private TextureRenderer _textureRenderer = null!;
    
    public TriggerScriptTest() : base(true) {
    }

    public override void Awake() {
        _textureRenderer = GetComponent<TextureRenderer>();
    }

    public override void OnMouseEnter() {
        _textureRenderer.color = Color.RED;
    }

    public override void OnMouseExit() {
        _textureRenderer.color = Color.WHITE;
    }
}