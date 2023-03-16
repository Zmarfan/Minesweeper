using Worms.engine.data;
using Worms.engine.game_object.scripts;
using Worms.engine.helper;

namespace Worms.game.asteroids.saucer; 

public class SaucerMovement : Script {
    private const float VELOCITY = 230f;
    private static readonly List<Vector2> POSSIBLE_DIRECTIONS = ListUtils.Of(
        Vector2.Right(), 
        new Vector2(1, 1).Normalized, 
        new Vector2(1, -1).Normalized
    );
    private static readonly Random RANDOM = new();

    private readonly ClockTimer _directionChangeTimer = new(2);
    private int _directionIndex = 0;
    private readonly bool _right;
    
    public SaucerMovement(bool right) : base(true) {
        _right = right;
    }

    public override void Update(float deltaTime) {
        _directionChangeTimer.Time += deltaTime;
        if (_directionChangeTimer.Expired()) {
            _directionIndex = RANDOM.Next(POSSIBLE_DIRECTIONS.Count);
            _directionChangeTimer.Reset();
        }
    }

    public override void FixedUpdate(float deltaTime) {
        Transform.Position += POSSIBLE_DIRECTIONS[_directionIndex] * VELOCITY * deltaTime * (_right ? 1 : -1);
    }
}