using GameEngine.engine.camera;
using GameEngine.engine.core.input;
using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.scripts;
using Minesweeper.minesweeper.game.menu;
using Minesweeper.minesweeper.game.number_display;
using Minesweeper.minesweeper.game.smiley;
using Button = GameEngine.engine.core.input.listener.Button;

namespace Minesweeper.minesweeper.game.board; 

public class Board : Script {
    public const int TILE_LENGTH = 89;
    public const int BORDER_LENGTH = 60;
    public const int INFO_HEIGHT = 175;
    public const int BOMB = -1;
    public const int OPENED_BOMB = -2;
    public const int WRONG_BOMB = -3;
    public const string MINE_NUMBER_DISPLAY = "mineNumberDisplay";
    public const string TIME_NUMBER_DISPLAY = "timeNumberDisplay";

    private readonly BoardSettings _settings;
    private Transform _tileHolder = null!;
    private Smiley _smiley = null!;
    private NumberDisplay _timeNumberDisplay = null!;
    private NumberDisplay _mineNumberDisplay = null!;
    private readonly Tile[,] _tiles;
    private Vector2Int? _hoveringTilePosition;
    private float _timePassed;
    private int _tilesFlagged;
    private readonly int _tilesToOpen;
    private int _openedTiles;
    private bool _gameOver;
    private bool _madeFirstMove;
    
    public Board(BoardSettings settings) {
        _settings = settings;
        _tiles = new Tile[settings.width, settings.height];
        _tilesToOpen = settings.width * settings.height - settings.mines;
    }

    public override void Awake() {
        _tileHolder = Transform.Instantiate(GameObjectBuilder
            .Builder("tileHolder")
            .SetLocalPosition(CalculateTileHolderPosition())
        ).Transform;
        BoardBackgroundFactory.Create(Transform, _settings.width, _settings.height);

        WindowManager.SetResolution(new Vector2Int(
            (int)((TILE_LENGTH * _settings.width + BORDER_LENGTH * 2) * (1 / Camera.Main.Size)),
            (int)((TILE_LENGTH * _settings.height + BORDER_LENGTH * 3 + INFO_HEIGHT) * (1 / Camera.Main.Size))
        ));

        _smiley = GetComponentInChildren<Smiley>();
        _smiley.Clicked += RestartGame;
    }

    public override void Start() {
        List<NumberDisplay> numberDisplays = GetComponentsInChildren<NumberDisplay>();
        _mineNumberDisplay = numberDisplays.First(display => display.Name == MINE_NUMBER_DISPLAY);
        _timeNumberDisplay = numberDisplays.First(display => display.Name == TIME_NUMBER_DISPLAY);
        RestartGame();
    }

    public override void Update(float deltaTime) {
        _hoveringTilePosition = CalculateHoveringTilePosition();

        _mineNumberDisplay.DisplayNumber(_settings.mines - _tilesFlagged);
        _timeNumberDisplay.DisplayNumber((int)_timePassed);
        if (!_gameOver && _madeFirstMove) {
            _timePassed += deltaTime;
        }

        if (Input.GetKeyDown(Button.F2)) {
            RestartGame();
        }

        if (!_hoveringTilePosition.HasValue) {
            return;
        }
        if (Input.GetKeyDown(Button.LEFT_MOUSE)) {
            TileLeftClicked(_hoveringTilePosition.Value);
        }
        if (Input.GetKeyDown(Button.RIGHT_MOUSE)) {
            TileRightClicked(_hoveringTilePosition.Value);
        }
    }

    private void RestartGame() {
        _smiley.Restart();
        _tilesFlagged = 0;
        foreach (Transform child in _tileHolder.children) {
            child.gameObject.Destroy();
        }
        BoardCreator.InitBoard(_settings.mines, in _tileHolder, in _tiles);
        _timePassed = 0;
        _openedTiles = 0;
        _madeFirstMove = false;
        _gameOver = false;
    }

    private Vector2 CalculateTileHolderPosition() {
        return new Vector2(-_settings.width, -_settings.height) / 2f * TILE_LENGTH
               + new Vector2(TILE_LENGTH, TILE_LENGTH) / 2f;
    }

    private void TileLeftClicked(Vector2Int position) {
        Tile tile = _tiles[position.x, position.y];
        if (_gameOver || tile.MarkType is MarkType.FLAGGED) {
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
        for (int x = Math.Max(position.x - 1, 0); x <= Math.Min(position.x + 1, _settings.width - 1); x++) {
            for (int y = Math.Max(position.y - 1, 0); y <= Math.Min(position.y + 1, _settings.height - 1); y++) {
                flagCount += _tiles[x, y].MarkType == MarkType.FLAGGED ? 1 : 0;
            }
        }

        if (flagCount != _tiles[position.x, position.y].SurroundingMineCount) {
            return;
        }
        
        for (int x = Math.Max(position.x - 1, 0); x <= Math.Min(position.x + 1, _settings.width - 1); x++) {
            for (int y = Math.Max(position.y - 1, 0); y <= Math.Min(position.y + 1, _settings.height - 1); y++) {
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
            for (int x = Math.Max(currentPosition.x - 1, 0); x <= Math.Min(currentPosition.x + 1, _settings.width - 1); x++) {
                for (int y = Math.Max(currentPosition.y - 1, 0); y <= Math.Min(currentPosition.y + 1, _settings.height - 1); y++) {
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
            LoseGame();
        }
        else {
            _openedTiles++;
            if (_openedTiles == _tilesToOpen) {
                WinGame();
            }
        }
    }

    private void LoseGame() {
        _gameOver = true;
        RevealAllTiles(false);
        _smiley.LostGame();
    }

    private void WinGame() {
        _gameOver = true;
        _mineNumberDisplay.DisplayNumber(0);
        RevealAllTiles(true);
        _smiley.WonGame();
        HighScoreManager.SaveIfBetter((int)_timePassed, _settings.gameType);
    }
    
    private void RevealAllTiles(bool win) {
        foreach (Tile tile in _tiles) {
            tile.Reveal(win);
        }
    }
    
    private Vector2Int? CalculateHoveringTilePosition() {
        Vector2 position = _tileHolder.WorldToLocalMatrix.ConvertPoint(Input.MouseWorldPosition);
        Vector2Int tilePosition = new((int)(position.x / TILE_LENGTH + 0.5f), (int)(position.y / TILE_LENGTH + 0.5f));
        if (tilePosition.x < 0 || tilePosition.y < 0 || tilePosition.x >= _settings.width || tilePosition.y >= _settings.height) {
            return null;
        }

        return tilePosition;
    }
}