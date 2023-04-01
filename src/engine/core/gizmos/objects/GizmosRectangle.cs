using SDL2;
using GameEngine.engine.data;
using Color = GameEngine.engine.data.Color;

namespace GameEngine.engine.core.gizmos.objects; 

internal readonly struct GizmosRectangle : IGizmosObject {
    private readonly Color _color;
    private readonly Vector2 _center;
    private readonly Vector2 _size;
    private readonly Rotation _rotation;

    public GizmosRectangle(Vector2 center, Vector2 size, Rotation rotation, Color color) {
        _color = color;
        _center = center;
        _size = size;
        _rotation = rotation;
    }

    public Color GetColor() {
        return _color;
    }

    public void Render(nint renderer, TransformationMatrix worldToScreenMatrix) {
        Vector2 bottomLeft = GetCorner(new Vector2(_center.x - _size.x / 2f, _center.y - _size.y / 2f), worldToScreenMatrix);
        Vector2 topLeft = GetCorner(new Vector2(_center.x - _size.x / 2f, _center.y + _size.y / 2f), worldToScreenMatrix);
        Vector2 bottomRight = GetCorner(new Vector2(_center.x + _size.x / 2f, _center.y - _size.y / 2f), worldToScreenMatrix);
        Vector2 topRight = GetCorner(new Vector2(_center.x + _size.x / 2f, _center.y + _size.y / 2f), worldToScreenMatrix);
        
        SDL.SDL_RenderDrawLineF(renderer, bottomLeft.x, bottomLeft.y, topLeft.x, topLeft.y);
        SDL.SDL_RenderDrawLineF(renderer, bottomLeft.x, bottomLeft.y, bottomRight.x, bottomRight.y);
        SDL.SDL_RenderDrawLineF(renderer, topRight.x, topRight.y, topLeft.x, topLeft.y);
        SDL.SDL_RenderDrawLineF(renderer, topRight.x, topRight.y, bottomRight.x, bottomRight.y);
    }

    private Vector2 GetCorner(Vector2 corner, TransformationMatrix worldToScreenMatrix) {
        return worldToScreenMatrix.ConvertPoint(Vector2.RotatePoint(_center, _rotation, corner));
    }
}