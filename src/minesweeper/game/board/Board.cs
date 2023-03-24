using GameEngine.engine.camera;
using GameEngine.engine.core.input;
using GameEngine.engine.core.input.listener;
using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.scripts;
using GameEngine.minesweeper.game.number_display;

namespace GameEngine.minesweeper.game.board; 

public class Board : Script {
    public const int TILE_LENGTH = 89;
    public const int BORDER_LENGTH = 60;
    public const int INFO_HEIGHT = 175;
    public const int BOMB = -1;
    public const int OPENED_BOMB = -2;
    public const int WRONG_BOMB = -3;
    public const string MINE_NUMBER_DISPLAY = "mineNumberDisplay";
    public const string TIME_NUMBER_DISPLAY = "timeNumberDisplay";

    private Transform _tileHolder = null!;
    private NumberDisplay _timeNumberDisplay = null!;
    private NumberDisplay _mineNumberDisplay = null!;
    private readonly Tile[,] _tiles;
    private float _timePassed;
    private readonly int _mineCount;
    private int _tilesFlagged;
    private readonly int _tilesToOpen;
    private int _openedTiles;
    private bool _gameOver;
    private bool _madeFirstMove;
    
    public Board(int width, int height, int mineCount) {
        _tiles = new Tile[width, height];
        _mineCount = mineCount;
        _tilesToOpen = width * height - mineCount;
        Tile.LeftClickedTileEvent += TileLeftClicked;
        Tile.RightClickedTileEvent += TileRightClicked;
    }

    public override void Awake() {
        _tileHolder = Transform.Instantiate(GameObjectBuilder
            .Builder("tileHolder")
            .SetLocalPosition(CalculateTileHolderPosition())
        ).Transform;
        GameObject background = BoardBackgroundFactory.Create(Transform, _tiles.GetLength(0), _tiles.GetLength(1));
        List<NumberDisplay> numberDisplays = background.GetComponentsInChildren<NumberDisplay>();
        _mineNumberDisplay = numberDisplays.First(display => display.Name == MINE_NUMBER_DISPLAY);
        _timeNumberDisplay = numberDisplays.First(display => display.Name == TIME_NUMBER_DISPLAY);
        
        WindowManager.SetResolution(new Vector2Int(
            (int)((TILE_LENGTH * _tiles.GetLength(0) + BORDER_LENGTH * 2) * (1 / Camera.Main.Size)),
            (int)((TILE_LENGTH * _tiles.GetLength(1) + BORDER_LENGTH * 3 + INFO_HEIGHT) * (1 / Camera.Main.Size))
        ));

        RestartGame();
    }

    public override void Update(float deltaTime) {
        if (!_gameOver) {
            _mineNumberDisplay.DisplayNumber(_mineCount - _tilesFlagged);
            if (_madeFirstMove) {
                _timePassed += deltaTime;
                _timeNumberDisplay.DisplayNumber((int)_timePassed);
            }
        }
        if (Input.GetKeyDown(Button.R)) {
            RestartGame();
        }
    }

    private void RestartGame() {
        _timeNumberDisplay.DisplayNumber(0);
        _tilesFlagged = 0;
        foreach (Transform child in _tileHolder.children) {
            child.gameObject.Destroy();
        }
        BoardCreator.InitBoard(_mineCount, in _tileHolder, in _tiles);
        _timePassed = 0;
        _openedTiles = 0;
        _madeFirstMove = false;
        _gameOver = false;
    }

    private Vector2 CalculateTileHolderPosition() {
        return new Vector2(-_tiles.GetLength(0), -_tiles.GetLength(1)) / 2f * TILE_LENGTH
               + new Vector2(TILE_LENGTH, TILE_LENGTH) / 2f;
    }

    private void TileLeftClicked(Vector2Int position) {
        Tile tile = _tiles[position.x, position.y];
        if (_gameOver) {
            return;
        }

        _madeFirstMove = true;
        if (tile.MarkType == MarkType.OPENED) {
            OpenTilesFromOpenTile(position);
        }

        OpenTile(tile);
        if (tile.IsEmpty) {
            OpenEmptyTiles(position);
        }
    }

    private void TileRightClicked(Vector2Int position) {
        if (_gameOver) {
            return;
        }

        Tile tile = _tiles[position.x, position.y];
        if (tile.MarkType == MarkType.FLAGGED) {
            _tilesFlagged--;
        }
        tile.Flag();
        if (tile.MarkType == MarkType.FLAGGED) {
            _tilesFlagged++;
        }
    }
    
    private void OpenTilesFromOpenTile(Vector2Int position) {
        int flagCount = 0;
        for (int x = Math.Max(position.x - 1, 0); x <= Math.Min(position.x + 1, _tiles.GetLength(0) - 1); x++) {
            for (int y = Math.Max(position.y - 1, 0); y <= Math.Min(position.y + 1, _tiles.GetLength(0) - 1); y++) {
                flagCount += _tiles[x, y].MarkType == MarkType.FLAGGED ? 1 : 0;
            }
        }

        if (flagCount != _tiles[position.x, position.y].SurroundingMineCount) {
            return;
        }
        
        for (int x = Math.Max(position.x - 1, 0); x <= Math.Min(position.x + 1, _tiles.GetLength(0) - 1); x++) {
            for (int y = Math.Max(position.y - 1, 0); y <= Math.Min(position.y + 1, _tiles.GetLength(0) - 1); y++) {
                if (_tiles[x, y].MarkType == MarkType.FLAGGED) {
                    continue;
                }
                OpenTile(_tiles[x, y]);
                if (_tiles[x, y].IsEmpty) {
                    OpenEmptyTiles(new Vector2Int(x, y));
                }
            }
        }
    }
    
    private void OpenEmptyTiles(Vector2Int position) {
        Queue<Vector2Int> emptyTiles = new();
        emptyTiles.Enqueue(position);

        while (emptyTiles.Count > 0) {
            Vector2Int currentPosition = emptyTiles.Dequeue();
            for (int x = Math.Max(currentPosition.x - 1, 0); x <= Math.Min(currentPosition.x + 1, _tiles.GetLength(0) - 1); x++) {
                for (int y = Math.Max(currentPosition.y - 1, 0); y <= Math.Min(currentPosition.y + 1, _tiles.GetLength(1) - 1); y++) {
                    Tile tile = _tiles[x, y];
                    if (tile.MarkType == MarkType.OPENED || (currentPosition.x == x && currentPosition.y == y)) {
                        continue;
                    }

                    OpenTile(tile);
                    if (tile.IsEmpty) {
                        emptyTiles.Enqueue(new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    private void OpenTile(Tile tile) {
        if (tile.MarkType == MarkType.OPENED) {
            return;
        }
        tile.Open();
        if (tile.IsBomb) {
            EndGameRevealAllTiles(false);
        }
        else {
            _openedTiles++;
            if (_openedTiles == _tilesToOpen) {
                _mineNumberDisplay.DisplayNumber(0);
                EndGameRevealAllTiles(true);
            }
        }
    }

    private void EndGameRevealAllTiles(bool win) {
        _gameOver = true;
        foreach (Tile tile in _tiles) {
            tile.Reveal(win);
        }
    }
}