using GameEngine.engine.camera;
using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.scripts;

namespace GameEngine.minesweeper.game.board; 

public class Board : Script {
    private Transform _tileHolder = null!;
    private readonly Tile[,] _tiles;

    public Board(int width, int height) {
        _tiles = new Tile[width, height];
    }

    public override void Awake() {
        _tileHolder = Transform.Instantiate(GameObjectBuilder
            .Builder("tileHolder")
            .SetLocalPosition(CalculateTileHolderPosition())
        ).Transform;
        
        WindowManager.SetResolution(new Vector2Int(
            (int)(BoardCreator.TILE_LENGTH * _tiles.GetLength(0) * (1 / Camera.Main.Size)),
            (int)(BoardCreator.TILE_LENGTH * _tiles.GetLength(1) * (1 / Camera.Main.Size))
        ));

        BoardCreator.InitBoard(50, in _tileHolder, in _tiles);
    }

    private Vector2 CalculateTileHolderPosition() {
        return new Vector2(-_tiles.GetLength(0), -_tiles.GetLength(1)) / 2f * BoardCreator.TILE_LENGTH
               + new Vector2(BoardCreator.TILE_LENGTH, BoardCreator.TILE_LENGTH) / 2f;
    }
}