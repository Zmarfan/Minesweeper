using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.game_object.scripts;

namespace Minesweeper.minesweeper.game.number_display; 

public class NumberDisplay : Script {
    public const string NO_0 = "no0";
    public const string NO_1 = "no1";
    public const string NO_2 = "no2";

    private const int NEGATIVE_SIGN = -1;
    private const int UNLIT = 100;
    
    private TextureRenderer _no0 = null!;
    private TextureRenderer _no1 = null!;
    private TextureRenderer _no2 = null!;

    private readonly int[] _displayParts = new int[3];

    public NumberDisplay(string name) : base(true, name) {
    }

    public override void Start() {
        List<TextureRenderer> textureRenderers = GetComponentsInChildren<TextureRenderer>();
        _no0 = textureRenderers.First(tr => tr.Name == NO_0);
        _no1 = textureRenderers.First(tr => tr.Name == NO_1);
        _no2 = textureRenderers.First(tr => tr.Name == NO_2);
    }

    public void DisplayNumber(int number) {
        switch (number) {
            case > 999:
                SetNumbers(9, 9, 9);
                return;
            case < -99:
                SetNumbers(NEGATIVE_SIGN, 9, 9);
                return;
            default:
                CalculateNumberAsParts(number);
                SetNumbers(_displayParts[0], _displayParts[1], _displayParts[2]);
                return;
        }
    }

    private void SetNumbers(int no0, int no1, int no2) {
        _no0.texture = TextureProvider.GetNumberTexture(no0);
        _no1.texture = TextureProvider.GetNumberTexture(no1);
        _no2.texture = TextureProvider.GetNumberTexture(no2);
    }

    private void CalculateNumberAsParts(int number) {
        bool negative = number < 0;
        _displayParts[0] = UNLIT;
        _displayParts[1] = negative ? NEGATIVE_SIGN : UNLIT;
        _displayParts[2] = 0;

        number = Math.Abs(number);
        int i = _displayParts.Length - 1;
        while (number > 0) {
            _displayParts[i--] = number % 10;
            number /= 10;
            if (i > 0 && number > 0 && negative) {
                _displayParts[i - 1] = NEGATIVE_SIGN;
            }
        }
    }
}