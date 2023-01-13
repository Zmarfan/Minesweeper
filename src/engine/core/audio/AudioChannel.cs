namespace Worms.engine.core.audio; 

public class AudioChannel {
    public readonly string name;

    public Volume Volume {
        get => _volume;
        set {
            AudioHandler.ReloadVolumeSettings();
            _volume = value;
        }
    }
    private Volume _volume;

    public AudioChannel(string name, Volume volume) {
        this.name = name;
        _volume = volume;
    }
}