using Worms.engine.core.update.physics;
using Worms.engine.data;

namespace Worms.engine.game_object.components.physics.colliders; 

public class PixelCollider : Collider {
    private const int NORMAL_CHECK_DEPTH = 4;
    private const int MIN_SURROUNDING_PIXELS_FOR_VALID_NORMAL = 16;
    
    public Color[,] pixels;

    public bool flipX;
    public bool flipY; 
    public int Width => pixels.GetLength(0);
    public int Height => pixels.GetLength(1);
    private int EvenWidthOffset => (Width + (flipX ? 0 : 1)) % 2;
    private int EvenHeightOffset => (Height + (flipY ? 0 : 1)) % 2;

    private int FlipXSign => flipX ? -1 : 1;
    private int FlipYSign => flipY ? 1 : -1;

    public PixelCollider(
        bool isActive,
        Color[,] pixels,
        bool flipX,
        bool flipY,
        ColliderState state,
        Vector2Int offset
    ) : base(isActive, state, new Vector2(offset.x, offset.y)) {
        this.flipX = flipX;
        this.flipY = flipY;
        this.pixels = pixels;
    }

    public override Tuple<Vector2, Vector2> GetWorldBoundingBox() {
        List<Vector2> corners = new() {
            PixelToLocal(new Vector2Int(0, 0)),
            PixelToLocal(new Vector2Int(Width - 1, 0)),
            PixelToLocal(new Vector2Int(0, Height - 1)),
            PixelToLocal(new Vector2Int(Width - 1, Height - 1))
        };
        corners = corners.Select(p => Transform.LocalToWorldMatrix.ConvertPoint(p)).ToList();
        
        float minX = corners.MinBy(p => p.x).x;
        float minY = corners.MinBy(p => p.y).y;
        float maxX = corners.MaxBy(p => p.x).x;
        float maxY = corners.MaxBy(p => p.y).y;
        return new Tuple<Vector2, Vector2>(new Vector2(minX, minY), new Vector2(maxX, maxY));
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        Vector2Int pixel = LocalToPixel(p);
        if (!PixelIsInTexture(pixel)) {
            return false;
        }
        return pixels[pixel.x, pixel.y].IsOpaque;
    }
    
    public override ColliderHit? Raycast(Vector2 origin, Vector2 direction) {
        Vector2Int from = LocalToPixel(Transform.WorldToLocalMatrix.ConvertPoint(origin));
        Vector2Int to = LocalToPixel(origin + Transform.WorldToLocalMatrix.ConvertVector(direction));

        if (PixelIsInTexture(from) && pixels[from.x, from.y].IsOpaque) {
            return null;
        }
        
        Vector2Int? pixel = CalculatePointLineHits(from, to);
        if (!pixel.HasValue) {
            return null;
        }

        return new ColliderHit(
            Transform.LocalToWorldMatrix.ConvertPoint(PixelToLocal(pixel.Value)),
            Transform.LocalToWorldMatrix.ConvertVector(CalculateNormal(pixel.Value)).Normalized
        );
    }

    public Vector2Int LocalToPixel(Vector2 p) {
        return new Vector2Int(
            FlipXSign * ((int)Math.Round(p.x) - (int)offset.x) + Width / 2 - EvenWidthOffset,
            FlipYSign * ((int)Math.Round(p.y) - (int)offset.y) + Height / 2 - EvenHeightOffset
        );
    }
    
    public Vector2 PixelToLocal(Vector2Int p) {
        return new Vector2(
            FlipXSign * (p.x - Width / 2f + EvenWidthOffset) + offset.x,
            FlipYSign * (p.y - Height / 2f + EvenHeightOffset) + offset.y
        );
    }
    
    private Vector2 CalculateNormal(Vector2Int pixel) {
        Vector2 point = new(pixel.x, -pixel.y);
        Vector2 normal = Vector2.Zero();
        int surroundingPixels = 0;
        
        for (int x = Math.Max(pixel.x - NORMAL_CHECK_DEPTH, 0); x <= Math.Min(pixel.x + NORMAL_CHECK_DEPTH, Width - 1); x++) {
            for (int y = Math.Max(pixel.y - NORMAL_CHECK_DEPTH, 0); y <= Math.Min(pixel.y + NORMAL_CHECK_DEPTH, Height - 1); y++) {
                if (!pixels[x, y].IsOpaque || (x == pixel.x && y == pixel.y)) {
                    continue;
                }

                surroundingPixels++;
                normal += point - new Vector2(x, -y);
            }
        }

        if (surroundingPixels < MIN_SURROUNDING_PIXELS_FOR_VALID_NORMAL) {
            return Vector2.Zero();
        }

        return normal;
    }

    private bool PixelIsInTexture(Vector2Int pixel) {
        return pixel.x >= 0 && pixel.x < Width && pixel.y >= 0 && pixel.y < Height;
    }

    private Vector2Int? CalculatePointLineHits(Vector2Int p1, Vector2Int p2) {
        Vector2Int dimensions = p2 - p1;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        dx1 = dimensions.x switch {
            < 0 => -1,
            > 0 => 1,
            _ => dx1
        };
        dy1 = dimensions.y switch {
            < 0 => -1,
            > 0 => 1,
            _ => dy1
        };
        dx2 = dimensions.x switch {
            < 0 => -1,
            > 0 => 1,
            _ => dx2
        };

        int longest = Math.Abs(dimensions.x) ;
        int shortest = Math.Abs(dimensions.y) ;
        
        if (!(longest > shortest)) {
            longest = Math.Abs(dimensions.y) ;
            shortest = Math.Abs(dimensions.x) ;
            dy2 = dimensions.y switch {
                < 0 => -1,
                > 0 => 1,
                _ => dy2
            };
            dx2 = 0;            
        }
        int numerator = longest >> 1 ;
        for (int i = 0; i <= longest; i++) {
            if (PixelIsInTexture(p1) && pixels[p1.x, p1.y].IsOpaque) {
                return p1;
            }
            numerator += shortest;
            if (!(numerator < longest)) {
                numerator -= longest;
                p1.x += dx1;
                p1.y += dy1;
            } else {
                p1.x += dx2;
                p1.y += dy2;
            }
        }

        return null;
    }
}