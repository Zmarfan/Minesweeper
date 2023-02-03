using System.Diagnostics;
using Worms.engine.core.gizmos;
using Worms.engine.data;

namespace Worms.engine.game_object.components.colliders; 

public class PixelCollider : Collider {
    public Color[,] pixels;
    private int Width => pixels.GetLength(0);
    private int Height => pixels.GetLength(1);
    private int EvenWidthOffset => (Width + 1) % 2;
    private int EvenHeightOffset => (Height + 1) % 2;

    public PixelCollider(
        bool isActive,
        Color[,] pixels,
        bool isTrigger,
        Vector2 offset
    ) : base(isActive, isTrigger, new Vector2((int)offset.x, (int)offset.y)) {
        this.pixels = pixels;
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        int pixelX = (int)Math.Round(p.x) - (int)offset.x + Width / 2 - EvenWidthOffset;
        int pixelY = Height / 2 - EvenHeightOffset - (int)Math.Round(p.y) - (int)offset.y;
        if (pixelX < 0 || pixelY < 0 || pixelX >= Width || pixelY >= Height) {
            return false;
        }
        return pixels[pixelX, pixelY].IsOpaque;
    }

    public override void OnDrawGizmos() {
    }
}