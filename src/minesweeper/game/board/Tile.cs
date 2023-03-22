using GameEngine.engine.data;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;

namespace GameEngine.minesweeper.game.board; 

public class Tile : Script {
    public MarkType MarkType { get; private set; } = MarkType.NONE;
    private readonly int _mineCount;

    private TextureRenderer _textureRenderer = null!;
    
    public Tile(int mineCount) {
        _mineCount = mineCount;
    }

    public override void Awake() {
        _textureRenderer = GetComponent<TextureRenderer>();
    }

    public override void OnMouseEnter() {
        _textureRenderer.Color = Color.BLUE;
    }

    public override void OnMouseExit() {
        _textureRenderer.Color = Color.WHITE;
    }
}