using System.Diagnostics;
using Worms.engine.core.gizmos;
using Worms.engine.data;

namespace Worms.engine.game_object.components.colliders; 

public class PixelCollider : Collider {
    public Color[,] pixels;

    public bool flipX;
    public bool flipY; 
    private int Width => pixels.GetLength(0);
    private int Height => pixels.GetLength(1);
    private int EvenWidthOffset => (Width + (flipX ? 0 : 1)) % 2;
    private int EvenHeightOffset => (Height + (flipY ? 0 : 1)) % 2;

    private int FlipXSign => flipX ? -1 : 1;
    private int FlipYSign => flipY ? 1 : -1;

    public PixelCollider(
        bool isActive,
        Color[,] pixels,
        bool flipX,
        bool flipY,
        bool isTrigger,
        Vector2 offset
    ) : base(isActive, isTrigger, new Vector2((int)offset.x, (int)offset.y)) {
        this.flipX = flipX;
        this.flipY = flipY;
        this.pixels = pixels;
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        int pixelX = CalculateXPixel(p.x);
        int pixelY = CalculateYPixel(p.y);
        if (pixelX < 0 || pixelY < 0 || pixelX >= Width || pixelY >= Height) {
            return false;
        }
        return pixels[pixelX, pixelY].IsOpaque;
    }

    private int CalculateXPixel(float x) {
        return FlipXSign * ((int)Math.Round(x) - (int)offset.x) + Width / 2 - EvenWidthOffset;
    }
    
    private int CalculateYPixel(float y) {
        return FlipYSign * ((int)Math.Round(y) - (int)offset.y) + Height / 2 - EvenHeightOffset;
    }
}