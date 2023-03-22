using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.helper;

namespace GameEngine.minesweeper.game.board; 

public static class BoardCreator {
    public const int TILE_LENGTH = 16;
    public const int BOMB = -1;
    public const int OPENED_BOMB = -2;
    public const int WRONG_BOMB = -3;
    
    public static void InitBoard(int mineCount, in Transform holder, in Tile[,] tiles) {
        HashSet<Vector2Int> mines = CreateMinePositions(mineCount, in tiles);
        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++) {
                Vector2Int position = new(x, y);
                tiles[x, y] = CreateTile(holder, position, CalculateMinesAroundTile(position, mines, in tiles));
            }
        }
    }
    
    private static HashSet<Vector2Int> CreateMinePositions(int mineCount, in Tile[,] tiles) {
        List<Vector2Int> allPositions = new();
        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++) {
                allPositions.Add(new Vector2Int(x, y));
            }
        }
        RandomUtil.RandomizeList(ref allPositions);
        return allPositions.Take(mineCount).ToHashSet();
    }
    
    private static int CalculateMinesAroundTile(Vector2Int position, HashSet<Vector2Int> mines, in Tile[,] tiles) {
        if (mines.Contains(position)) {
            return BOMB;
        }

        int mineCount = 0;
        for (int x = Math.Max(position.x - 1, 0); x <= Math.Min(position.x + 1, tiles.GetLength(0) - 1); x++) {
            for (int y = Math.Max(position.y - 1, 0); y <= Math.Min(position.y + 1, tiles.GetLength(0) - 1); y++) {
                mineCount += mines.Contains(new Vector2Int(x, y)) ? 1 : 0;
            }
        }

        return mineCount;
    }
    
    private static Tile CreateTile(Transform holder, Vector2Int position, int mineCount) {
        return holder.Instantiate(GameObjectBuilder
            .Builder($"tile: {position.x}, {position.y})")
            .SetComponent(TextureRendererBuilder.Builder(TileProvider.GetTexture(mineCount, MarkType.NONE)).Build())
            .SetComponent(new BoxCollider(true, ColliderState.TRIGGER, new Vector2(TILE_LENGTH, TILE_LENGTH), Vector2.Zero()))
            .SetComponent(new Tile(mineCount))
            .SetLocalPosition(new Vector2(position.x * TILE_LENGTH, position.y * TILE_LENGTH))
        ).GetComponent<Tile>();
    }
}