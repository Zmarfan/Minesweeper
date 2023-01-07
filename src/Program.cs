using Worms.engine;
using Worms.engine.core;

namespace Worms;

internal static class Program {
    private static void Main() {
        Game game = new("test game", 600, 400);
        game.Run();
    }
}