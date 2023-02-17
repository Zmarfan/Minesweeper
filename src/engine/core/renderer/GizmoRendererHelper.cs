using SDL2;
using Worms.engine.data;

namespace Worms.engine.core.renderer; 

public static class GizmoRendererHelper {
    public static void DrawEllipse(nint renderer, Vector2 center, Vector2 radius, Rotation rotation) {
        float sin = (float)Math.Sin(rotation.Radians);
        float cos = (float)Math.Cos(rotation.Radians);
        float sinInv = (float)Math.Sin(-rotation.Radians);
        float cosInv = (float)Math.Cos(-rotation.Radians);
        
        int x = 0;
        int y = (int)radius.y;
 
        float d1 = radius.y * radius.y - radius.x * radius.x * radius.y + 0.25f * radius.x * radius.x;
        float dx = 2 * radius.y * radius.y * x;
        float dy = 2 * radius.x * radius.x * y;
     
        while (dx < dy) {
            RenderEllipsisPixels(renderer, center, x, y, cos, sin, cosInv, sinInv);

            if (d1 < 0)
            {
                x++;
                dx += 2 * radius.y * radius.y;
                d1 = d1 + dx + radius.y * radius.y;
            }
            else
            {
                x++;
                y--;
                dx += 2 * radius.y * radius.y;
                dy -= 2 * radius.x * radius.x;
                d1 = d1 + dx - dy + radius.y * radius.y;
            } 
        }
 
        float d2 = radius.y * radius.y * ((x + 0.5f) * (x + 0.5f)) + radius.x * radius.x * ((y - 1) * (y - 1)) - radius.x * radius.x * radius.y * radius.y;
        
        while (y >= 0)
        {
            RenderEllipsisPixels(renderer, center, x, y, cos, sin, cosInv, sinInv);
            
            if (d2 > 0)
            {
                y--;
                dy -= 2 * radius.x * radius.x;
                d2 = d2 + radius.x * radius.x - dy;
            }
            else
            {
                y--;
                x++;
                dx += 2 * radius.y * radius.y;
                dy -= 2 * radius.x * radius.x;
                d2 = d2 + dx - dy + radius.x * radius.x;
            }
        }
    }

    private static void RenderEllipsisPixels(nint renderer, Vector2 center, float x, float y, float cos, float sin, float cosInv, float sinInv) {
        Vector2 p = new(x * cos - y * sin, x * sin + y * cos);
        Vector2 p2 = new(x * cosInv - y * sinInv, x * sinInv + y * cosInv);
        SDL.SDL_RenderDrawPointF(renderer, center.x + p2.x, center.y - p2.y);
        SDL.SDL_RenderDrawPointF(renderer, center.x + p.x, center.y + p.y);
        SDL.SDL_RenderDrawPointF(renderer, center.x - p.x, center.y - p.y);
        SDL.SDL_RenderDrawPointF(renderer, center.x - p2.x, center.y + p2.y);
    }
}