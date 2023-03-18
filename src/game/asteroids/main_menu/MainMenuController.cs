using Worms.engine.core.input;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.main_menu; 

public class MainMenuController : Script {
    public const string PLAY = "PLAY GAME";
    public const string SCORE = "HIGH SCORES";
    
    private bool _selectedPlay = true;
    private TextRenderer _playRenderer = null!;
    private TextRenderer _scoreRenderer = null!;
    
    public override void Awake() {
        _playRenderer = GetComponentsInChildren<TextRenderer>().First(r => r.Name == PLAY);
        _scoreRenderer = GetComponentsInChildren<TextRenderer>().First(r => r.Name == SCORE);
    }

    public override void Update(float deltaTime) {
        bool selected = Input.GetButtonDown(InputNames.MENU_SELECT);
        if (selected && _selectedPlay) {
            SceneManager.LoadScene(SceneNames.GAME);
        }
        HandleMenuNavigation();
    }

    private void HandleMenuNavigation() {
        if (Input.GetButtonDown(InputNames.MENU_UP) || Input.GetButtonDown(InputNames.MENU_DOWN)) {
            _selectedPlay = !_selectedPlay;
        }
        
        if (_selectedPlay) {
            _playRenderer.Text = $"- {PLAY} -";
            _scoreRenderer.Text = SCORE;
        }
        else {
            _playRenderer.Text = PLAY;
            _scoreRenderer.Text = $"- {SCORE} -";
        }
    }
}