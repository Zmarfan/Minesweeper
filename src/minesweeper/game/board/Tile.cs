using GameEngine.engine.core.update.physics.updating;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;

namespace GameEngine.minesweeper.game.board; 

public class Tile : Script {
    public delegate void ClickedTileDelegate(Vector2Int position);
    public static event ClickedTileDelegate? LeftClickedTileEvent;
    public static event ClickedTileDelegate? RightClickedTileEvent;

    public bool IsBomb => _surroundingMineCount < 0;
    public bool IsEmpty => _surroundingMineCount == 0;
    public bool IsOpen => _markType == MarkType.OPENED;

    private int _surroundingMineCount;
    private MarkType _markType = MarkType.NONE;
    private readonly Vector2Int _position;

    private TextureRenderer _textureRenderer = null!;

    public Tile(Vector2Int position, int surroundingMineCount) {
        _position = position;
        _surroundingMineCount = surroundingMineCount;
    }

    public override void Awake() {
        _textureRenderer = GetComponent<TextureRenderer>();
    }

    public override void OnMouseDown(MouseClickMask mask) {
        if (mask.LeftClick && _markType != MarkType.FLAGGED || _markType == MarkType.OPENED) {
            LeftClickedTileEvent?.Invoke(_position);
        } 
        else if (mask.RightClick) {
            RightClickedTileEvent?.Invoke(_position);
        } 
    }

    public void Open() {
        _markType = MarkType.OPENED;
        if (IsBomb) {
            _surroundingMineCount = Board.OPENED_BOMB;
        }
        RefreshTexture();
    }

    public void Flag() {
        _markType = _markType switch {
            MarkType.NONE => MarkType.FLAGGED,
            MarkType.FLAGGED => MarkType.QUESTION_MARK,
            MarkType.QUESTION_MARK => MarkType.NONE,
            _ => _markType
        };
        RefreshTexture();
    }
    
    public void Reveal() {
        if (_markType == MarkType.FLAGGED && !IsBomb) {
            _surroundingMineCount = Board.WRONG_BOMB;
        }
        else if (_markType == MarkType.FLAGGED) {
            return;
        }
        
        _markType = MarkType.OPENED;
        RefreshTexture();
    }

    private void RefreshTexture() {
        _textureRenderer.texture = TileProvider.GetTexture(_surroundingMineCount, _markType);
    }
}