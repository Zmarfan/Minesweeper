using Worms.engine.core;
using Worms.game.asteroids;

namespace Worms;

internal static class Program {
    private static void Main() {
        Game game = Asteroids.CreateGame();
        game.Run();
    }
}