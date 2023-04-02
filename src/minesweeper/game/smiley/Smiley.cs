using GameEngine.engine.core.input;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;
using Button = GameEngine.engine.core.input.listener.Button;

namespace Minesweeper.minesweeper.game.smiley; 

public class Smiley : Script {
    public delegate void ClickedDelegate();
    public event ClickedDelegate? Clicked;

    public const float LENGTH = 134;

    private BoxCollider _boxCollider = null!;
    private TextureRenderer _textureRenderer = null!;
    private bool _freezeFace;
    
    public override void Awake() {
        _boxCollider = GetComponent<BoxCollider>();
        _textureRenderer = GetComponent<TextureRenderer>();
    }

    public override void Update(float deltaTime) {
        bool mouseInside = _boxCollider.IsPointInside(Input.MouseWorldPosition);
        
        if (Input.GetKeyUp(Button.LEFT_MOUSE) && mouseInside) {
            Clicked?.Invoke();
        }
        else if (Input.GetKey(Button.LEFT_MOUSE) && mouseInside) {
            ChangeSmiley(SmileyType.DEFAULT_PRESSED);
            _freezeFace = false;
        }
        else if (!_freezeFace && (!Input.GetKey(Button.LEFT_MOUSE) || !mouseInside)) {
            ChangeSmiley(SmileyType.DEFAULT);
        }
    }

    public void Restart() {
        _freezeFace = false;
    }

    public void WonGame() {
        ChangeSmiley(SmileyType.WON);
        _freezeFace = true;
    }

    public void LostGame() {
        ChangeSmiley(SmileyType.LOST);
        _freezeFace = true;
    }

    private void ChangeSmiley(SmileyType type) {
        _textureRenderer.texture = TextureProvider.GetSmiley(type);
    }
}