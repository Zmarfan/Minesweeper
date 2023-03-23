using GameEngine.engine.camera;
using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.scripts;

namespace GameEngine.minesweeper.game.board; 

public class Board : Script {
    public const int TILE_LENGTH = 89;
    public const int BOMB = -1;
    public const int OPENED_BOMB = -2;
    public const int WRONG_BOMB = -3;
    
    private Transform _tileHolder = null!;
    private readonly Tile[,] _tiles;
    private readonly int _mineCount;
    private bool _gameOver = false;
    
    public Board(int width, int height, int mineCount) {
        _tiles = new Tile[width, height];
        _mineCount = mineCount;
        Tile.LeftClickedTileEvent += TileLeftClicked;
        Tile.RightClickedTileEvent += TileRightClicked;
    }

    public override void Awake() {
        _tileHolder = Transform.Instantiate(GameObjectBuilder
            .Builder("tileHolder")
            .SetLocalPosition(CalculateTileHolderPosition())
        ).Transform;
        
        WindowManager.SetResolution(new Vector2Int(
            (int)(TILE_LENGTH * _tiles.GetLength(0) * (1 / Camera.Main.Size)),
            (int)(TILE_LENGTH * _tiles.GetLength(1) * (1 / Camera.Main.Size))
        ));

        BoardCreator.InitBoard(_mineCount, in _tileHolder, in _tiles);
    }

    private Vector2 CalculateTileHolderPosition() {
        return new Vector2(-_tiles.GetLength(0), -_tiles.GetLength(1)) / 2f * TILE_LENGTH
               + new Vector2(TILE_LENGTH, TILE_LENGTH) / 2f;
    }

    private void TileLeftClicked(Vector2Int position) {
        if (_gameOver) {
            return;
        }
        // Add first click safe mechanics later

        Tile tile = _tiles[position.x, position.y];
        tile.Open();
        if (tile.IsBomb) {
            LoseGame();
        }
        else if (tile.IsEmpty) {
            OpenEmptyTiles(position);
        }
    }
    
    private void TileRightClicked(Vector2Int position) {
        if (_gameOver) {
            return;
        }
        
        _tiles[position.x, position.y].Flag();
    }
    
    private void OpenEmptyTiles(Vector2Int position) {
        Queue<Vector2Int> emptyTiles = new();
        emptyTiles.Enqueue(position);

        while (emptyTiles.Count > 0) {
            Vector2Int currentPosition = emptyTiles.Dequeue();
            for (int x = Math.Max(currentPosition.x - 1, 0); x <= Math.Min(currentPosition.x + 1, _tiles.GetLength(0) - 1); x++) {
                for (int y = Math.Max(currentPosition.y - 1, 0); y <= Math.Min(currentPosition.y + 1, _tiles.GetLength(1) - 1); y++) {
                    Tile tile = _tiles[x, y];
                    if (tile.IsOpen || (currentPosition.x == x && currentPosition.y == y)) {
                        continue;
                    }

                    tile.Open();
                    if (tile.IsEmpty) {
                        emptyTiles.Enqueue(new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    private void LoseGame() {
        _gameOver = true;
        foreach (Tile tile in _tiles) {
            tile.Reveal();
        }
    }
}