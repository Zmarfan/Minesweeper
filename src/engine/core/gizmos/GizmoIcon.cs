using SDL2;
using Worms.engine.core.renderer;
using Worms.engine.data;

namespace Worms.engine.core.gizmos; 

public class GizmoIcon : GizmosObject {
    private static readonly Color BORDER_COLOR = Color.BLACK;
    private static readonly PixelType[,] TEMPLATE = {
        { PixelType.NONE, PixelType.NONE, PixelType.BORDER, PixelType.BORDER, PixelType.BORDER, PixelType.BORDER, PixelType.NONE, PixelType.NONE },
        { PixelType.NONE, PixelType.BORDER, PixelType.BORDER, PixelType.SOLID, PixelType.SOLID, PixelType.BORDER, PixelType.BORDER, PixelType.NONE },
        { PixelType.BORDER, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.BORDER, PixelType.BORDER },
        { PixelType.BORDER, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.BORDER },
        { PixelType.BORDER, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.BORDER },
        { PixelType.BORDER, PixelType.BORDER, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.SOLID, PixelType.BORDER, PixelType.BORDER },
        { PixelType.NONE, PixelType.BORDER, PixelType.BORDER, PixelType.SOLID, PixelType.SOLID, PixelType.BORDER, PixelType.BORDER, PixelType.NONE },
        { PixelType.NONE, PixelType.NONE, PixelType.BORDER, PixelType.BORDER, PixelType.BORDER, PixelType.BORDER, PixelType.NONE, PixelType.NONE },
    };
    
    private readonly Vector2 _center;

    public GizmoIcon(Vector2 center, Color color) : base(color) {
        _center = center;
    }

    public override void Render(nint renderer, TransformationMatrix worldToScreenMatrix) {
        SDL.SDL_GetRenderDrawColor(renderer, out byte r, out byte g, out byte b, out byte a);
        Vector2 centerScreen = worldToScreenMatrix.ConvertPoint(_center);
        for (int x = 0; x < TEMPLATE.GetLength(0); x++) {
            for (int y = 0; y < TEMPLATE.GetLength(1); y++) {
                switch (TEMPLATE[x, y]) {
                    case PixelType.NONE:
                        continue;
                    case PixelType.BORDER:
                        SDL.SDL_SetRenderDrawColor(renderer, BORDER_COLOR.Rbyte, BORDER_COLOR.Gbyte, BORDER_COLOR.Bbyte, BORDER_COLOR.Abyte);
                        break;
                    case PixelType.SOLID:
                        SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                SDL.SDL_RenderDrawPointF(renderer, centerScreen.x + x - 4, centerScreen.y + y - 4);
            }
        }
        SDL.SDL_SetRenderDrawColor(renderer, r, g, b, a);
    }

    private enum PixelType {
        NONE,
        BORDER,
        SOLID
    }
}