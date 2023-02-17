using Worms.engine.camera;
using Worms.engine.core.gizmos;
using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.core.update.physics;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyTestScript : Script {
    private const float SPEED = 4.5f;
    private readonly Func<GameObjectBuilder> _explosion;
    private AudioSource _audioSource = null!;

    private TextRenderer _textRenderer = null!;
    private RaycastHit? _hit;

    public MyTestScript(Func<GameObjectBuilder> explosion) : base(true) {
        _explosion = explosion;
    }

    public override void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _textRenderer = GetComponent<TextRenderer>();
    }

    public override void Update(float deltaTime) {
        if (Input.GetKeyDown(Button.TAB)) {
            Random random = new();
            Camera.Main.defaultDrawColor = new Color(random.NextSingle(), random.NextSingle(), random.NextSingle(), 1f);
        }
        
        if (Input.GetButtonDown("explosion")) {
            Transform.Instantiate(_explosion.Invoke().SetPosition(new Vector2(0, 100)));
            _audioSource.Restart();
            _textRenderer.lineSpacing += 5;
        }
        
        Transform.Position += Input.GetAxis("horizontal") * SPEED * 100 * deltaTime;
        Transform.Position += Input.GetAxis("vertical") * SPEED * 100 * deltaTime;
        Transform.Rotation += Input.GetButton("action") ? SPEED * 50 * deltaTime : 0;

        Physics.LineCast(Transform.Position, Input.MouseWorldPosition, out _hit);
    }

    public override void OnTriggerEnter(Collider collider) {
        Console.WriteLine($"enter: {collider.gameObject.Name}");
    }
    
    public override void OnTriggerExit(Collider collider) {
        Console.WriteLine($"exit: {collider.gameObject.Name}");
    }

    public override void OnDrawGizmos() {
        Gizmos.DrawRay(Transform.Position, Input.MouseWorldPosition - Transform.Position, Color.WHITE);
        if (_hit.HasValue) {
            Gizmos.DrawIcon(_hit.Value.point, Color.BLUE);
            Gizmos.DrawRay(_hit.Value.point, _hit.Value.normal * 50f, Color.ORANGE);
        }
    }
}