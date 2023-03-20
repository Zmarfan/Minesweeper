using GameEngine.engine.core.gizmos;
using GameEngine.engine.core.update.physics;
using GameEngine.engine.core.update.physics.updating;
using GameEngine.engine.data;

namespace GameEngine.engine.game_object.components.physics.colliders; 

public class PolygonCollider : Collider {
    public Vector2[] Vertices {
        get => _vertices;
        set {
            if (_vertices == value) {
                return;
            }

            _vertices = value;
            _triangulation = CalculateTriangulation(_vertices.ToList());
        }
    }

    public Vector2[] WorldVertices => Vertices.Select(v => Transform.LocalToWorldMatrix.ConvertPoint(v + offset)).ToArray();

    public IEnumerable<Vector2[]> Triangulation => _triangulation.Select(triangle => triangle.Select(v => Transform.LocalToWorldMatrix.ConvertPoint(v + offset)).ToArray());

    private List<Vector2[]> _triangulation = null!;
    private Vector2[] _vertices = null!;
    
    public PolygonCollider(
        bool isActive,
        Vector2[] vertices,
        ColliderState state,
        Vector2 offset
    ) : base(isActive, state, offset) {
        Vertices = vertices;
    }

    public override Vector2[] GetLocalCorners() {
        float minX = Vertices.MinBy(v => v.x).x + offset.x;
        float maxX = Vertices.MaxBy(v => v.x).x + offset.x;
        float minY = Vertices.MinBy(v => v.y).y + offset.y;
        float maxY = Vertices.MaxBy(v => v.y).y + offset.y;
        localCorners[0] = new Vector2(minX, minY);
        localCorners[1] = new Vector2(minX, maxY);
        localCorners[2] = new Vector2(maxX, maxY);
        localCorners[3] = new Vector2(maxX, minY);
        return localCorners;
    }

    public override bool IsPointInside(Vector2 p) {
        p = Transform.WorldToLocalMatrix.ConvertPoint(p);
        return IsPointInsidePolygon(Vertices, p);
    }

    public override ColliderHit? Raycast(Vector2 origin, Vector2 direction) {
        if (IsPointInside(origin)) {
            return null;
        }
        
        origin = Transform.WorldToLocalMatrix.ConvertPoint(origin);
        direction = Transform.WorldToLocalMatrix.ConvertVector(direction);

        if (PhysicsUtils.LinePolygonIntersectionWithNormal(Vertices, origin, direction, offset, out Tuple<Vector2, Vector2> value)) {
            return new ColliderHit(
                Transform.LocalToWorldMatrix.ConvertPoint(value.Item1),
                Transform.LocalToWorldMatrix.ConvertVector(value.Item2).Normalized
            );
        }

        return null;
    }

    private static List<Vector2[]> CalculateTriangulation(List<Vector2> vertices) {
        List<Vector2[]> triangles = new();

        while (vertices.Count >= 3) {
            int earIndex = FindEar(vertices);

            if (earIndex == -1) {
                throw new Exception("Polygon is not simple (i.e. it has holes or self-intersections)");
            }

            Vector2[] triangle = {
                vertices[earIndex],
                vertices[(earIndex + 1) % vertices.Count],
                vertices[(earIndex + 2) % vertices.Count]
            };

            triangles.Add(triangle);

            vertices.RemoveAt((earIndex + 1) % vertices.Count);
        }

        return triangles;
    }

    private static int FindEar(IReadOnlyList<Vector2> polygon) {
        for (int i = 0; i < polygon.Count; i++) {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Count];
            Vector2 c = polygon[(i + 2) % polygon.Count];
            
            if (IsConvex(a, b, c) && IsEar(a, b, c, polygon)) {
                return i;
            }
        }

        return -1;
    }
    
    private static bool IsConvex(Vector2 a, Vector2 b, Vector2 c) {
        return Vector2.Cross(a - b, c - b) > 0;
    }

    private static bool IsEar(Vector2 a, Vector2 b, Vector2 c, IEnumerable<Vector2> polygon) {
        return polygon
            .Where(t => t != a && t != b && t != c)
            .All(t => !IsPointInsidePolygon(new []{ a, b, c }, t));
    }

    private static bool IsPointInsidePolygon(IReadOnlyList<Vector2> polygon, Vector2 p) {
        int i, j, c = 0;
        for (i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++) {
            if (polygon[i].y > p.y != polygon[j].y > p.y
                && p.x < (polygon[j].x - polygon[i].x) * (p.y - polygon[i].y) / (polygon[j].y - polygon[i].y) +
                polygon[i].x
               ) {
                c = c == 0 ? 1 : 0;
            }
        }
        return c == 1;
    }
    
    public override void OnDrawGizmos() {
        foreach (Vector2[] triangle in _triangulation) {
            Gizmos.DrawPolygon(triangle.Select(v => Transform.LocalToWorldMatrix.ConvertPoint(v + offset)).ToList(), GizmoSettings.POLYGON_TRIANGLES_NAME);
        }
        Gizmos.DrawPolygon(WorldVertices, GizmoId);

        base.OnDrawGizmos();
    }
}