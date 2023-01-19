using Worms.engine.core.input;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.scripts;

namespace Worms.game; 

public class MyTestScript : Script {
    private readonly float _speed = 4.5f;
    private readonly Func<GameObjectBuilder> _explosion;
    private AudioSource _audioSource = null!;
    
    public MyTestScript(Func<GameObjectBuilder> explosion) : base(true) {
        _explosion = explosion;
    }

    public override void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Update(float deltaTime) {
        Transform.Position = Input.MouseWorldPosition;
        
        if (Input.GetButtonDown("explosion")) {
            Transform.Instantiate(_explosion.Invoke().SetPosition(new Vector2(0, 100)));
            _audioSource.Restart();
        }
        
        Transform.Position += Input.GetAxis("horizontal") * _speed * 100 * deltaTime;
        Transform.Position += Input.GetAxis("vertical") * _speed * 100 * deltaTime;
        Transform.Rotation += Input.GetButton("action") ? _speed * 50 * deltaTime : 0;
    }
}