using Worms.engine.game_object;

namespace Worms.engine.core; 

public record GameSettings(string title, int width, int height, GameObject root) {
    public string title = title;
    public int width = width;
    public int height = height;
    public GameObject root = root;
}