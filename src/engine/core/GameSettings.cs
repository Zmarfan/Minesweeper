using Worms.engine.camera;
using Worms.engine.game_object;

namespace Worms.engine.core; 

public record GameSettings(string title, int width, int height, Camera camera, GameObject root) {
    public readonly string title = title;
    public readonly int width = width;
    public readonly int height = height;
    public readonly Camera camera = camera;
    public readonly GameObject root = root;
}