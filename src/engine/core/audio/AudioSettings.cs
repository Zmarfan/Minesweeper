namespace Worms.engine.core.audio; 

public class AudioSettings {
    public Volume MasterVolume {
        get => _masterVolume;
        set {
            AudioHandler.ReloadVolumeSettings();
            _masterVolume = value;
        }
    }

    private Volume _masterVolume;
    private readonly Dictionary<string, AudioChannel> _audioChannels;

    public AudioSettings(Volume masterVolume, IEnumerable<AudioChannel> audioChannels) {
        _masterVolume = masterVolume;
        _audioChannels = audioChannels.ToDictionary(c => c.name, c => c);
    }

    public Volume GetChannelVolume(string channel) {
        return _audioChannels[channel].Volume;
    }
    
    public void SetChannelVolume(string channel, Volume volume) {
        _audioChannels[channel].Volume = volume;
    }

    public bool HasChannel(string channel) {
        return _audioChannels.ContainsKey(channel);
    }
}