using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game.asteroids.player; 

public class PlayerMovement : Script {
    private const float ROTATE_SPEED = 100;
    private const float THRUST_SPEED = 10;
    private const float MAX_THRUST_SPEED = 125;
    private const float DE_ACCELERATION_FRACTION = 0.975f;
    
    private Vector2 _velocity = Vector2.Zero();
    
    private float _rotateAmount;
    private float _thrust;

    private Texture _baseTexture;
    private Texture _thrustTexture;
    private TextureRenderer _textureRenderer = null!;
    
    public PlayerMovement() : base(true) {
    }

    public override void Awake() {
        _textureRenderer = GetComponent<TextureRenderer>();
        _baseTexture = _textureRenderer.texture;
        _thrustTexture = Texture.CreateMultiple(TextureNames.PLAYER, 0, 1, 1, 2);
    }

    public override void Update(float deltaTime) {
        _rotateAmount += Input.GetAxis(InputNames.ROTATE).x;
        _thrust += Input.GetAxis(InputNames.THRUST).x;

        _textureRenderer.texture = Input.GetButton(InputNames.THRUST) ? _thrustTexture : _baseTexture;
    }

    public override void FixedUpdate(float deltaTime) {
        Transform.Rotation += _rotateAmount * ROTATE_SPEED * deltaTime;
        _velocity *= DE_ACCELERATION_FRACTION;
        if (_thrust != 0) {
            _velocity += _thrust * Transform.Right;
        }
        if (_velocity.SqrMagnitude >= MAX_THRUST_SPEED * MAX_THRUST_SPEED) {
            _velocity = _velocity.Normalized * MAX_THRUST_SPEED;
        }

        Transform.LocalPosition += _velocity * THRUST_SPEED * deltaTime;
        
        _rotateAmount = 0;
        _thrust = 0;
    }
}