using GameEngine.engine.data;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;
using Minesweeper.minesweeper.game.menu;

namespace Minesweeper.minesweeper.game.board; 

public class Tile : Script {
    public bool IsBomb => SurroundingMineCount < 0;
    public bool IsEmpty => SurroundingMineCount == 0;

    public int SurroundingMineCount { get; private set; }
    public MarkType MarkType { get; private set; } = MarkType.NONE;

    private TextureRenderer _textureRenderer = null!;

    public Tile(int surroundingMineCount) {
        SurroundingMineCount = surroundingMineCount;
    }

    public override void Awake() {
        _textureRenderer = GetComponent<TextureRenderer>();
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