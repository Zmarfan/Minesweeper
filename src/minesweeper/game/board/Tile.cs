using GameEngine.engine.core.update.physics.updating;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;
using GameEngine.minesweeper.game.menu;

namespace GameEngine.minesweeper.game.board; 

public class Tile : Script {
    public delegate void ClickedTileDelegate(Vector2Int position);
    public static event ClickedTileDelegate? LeftClickedTileEvent;
    public static event ClickedTileDelegate? RightClickedTileEvent;

    public bool IsBomb => SurroundingMineCount < 0;
    public bool IsEmpty => SurroundingMineCount == 0;

    public int SurroundingMineCount { get; private set; }
    public MarkType MarkType { get; private set; } = MarkType.NONE;
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
        if (mask.LeftClick && MarkType != MarkType.FLAGGED || MarkType == MarkType.OPENED) {
            LeftClickedTileEvent?.Invoke(_position);
        } 
        else if (mask.RightClick) {
            RightClickedTileEvent?.Invoke(_position);
        } 
    }

    public void Open() {
        MarkType = MarkType.OPENED;
        if (IsBomb) {
            SurroundingMineCount = Board.OPENED_BOMB;
        }
        RefreshTexture();
    }

    public void Flag() {
        MarkType = MarkType switch {
            MarkType.NONE => MarkType.FLAGGED,
            MarkType.FLAGGED => MenuManager.UseQuestionMarks ? MarkType.QUESTION_MARK : MarkType.NONE,
            MarkType.QUESTION_MARK => MarkType.NONE,
            _ => MarkType
        };
        RefreshTexture();
    }
    
    public void Reveal(bool win) {
        if (MarkType == MarkType.FLAGGED && !IsBomb) {
            SurroundingMineCount = Board.WRONG_BOMB;
        }
        else if (MarkType == MarkType.FLAGGED) {
            return;
        }

        if (win && IsBomb) {
            MarkType = MarkType.FLAGGED;
        }
        else {
            MarkType = MarkType.OPENED;
        }
        
        RefreshTexture();
    }

    private void RefreshTexture() {
        _textureRenderer.texture = TextureProvider.GetTileTexture(SurroundingMineCount, MarkType);
    }
}