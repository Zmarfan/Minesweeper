using GameEngine.engine.core.update.physics.updating;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;

namespace GameEngine.minesweeper.game.board; 

public class Tile : Script {
    public delegate void ClickedTileDelegate(Vector2Int position);
    public static event ClickedTileDelegate? LeftClickedTileEvent;
    public static event ClickedTileDelegate? RightClickedTileEvent;

    public bool IsBomb => SurroundingMineCount < 0;
    public bool IsEmpty => SurroundingMineCount == 0;
    public bool IsOpen => _markType == MarkType.OPENED;
    public bool IsFlag => _markType == MarkType.FLAGGED;

    public int SurroundingMineCount { get; private set; }
    private MarkType _markType = MarkType.NONE;
    private readonly Vector2Int _position;

    private TextureRenderer _textureRenderer = null!;

    public Tile(Vector2Int position, int surroundingMineCount) {
        _position = position;
        SurroundingMineCount = surroundingMineCount;
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
            SurroundingMineCount = Board.OPENED_BOMB;
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
    
    public void Reveal(bool win) {
        if (_markType == MarkType.FLAGGED && !IsBomb) {
            SurroundingMineCount = Board.WRONG_BOMB;
        }
        else if (_markType == MarkType.FLAGGED) {
            return;
        }

        if (win && IsBomb) {
            _markType = MarkType.FLAGGED;
        }
        else {
            _markType = MarkType.OPENED;
        }
        
        RefreshTexture();
    }

    private void RefreshTexture() {
        _textureRenderer.texture = TileProvider.GetTexture(SurroundingMineCount, _markType);
    }
}