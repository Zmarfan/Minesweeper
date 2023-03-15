using Worms.engine.core.audio;

namespace Worms.engine.game_object.components.audio_source;

public class AudioSourceBuilder {
    private string _sourceName = "audio";
    private bool _isActive = true;
    private readonly string _audioId;
    private readonly string _channel;
    private bool _mute;
    private bool _loop;
    private bool _playOnAwake = true;
    private Volume _volume = Volume.Max();

    private AudioSourceBuilder(string audioId, string channel) {
        _audioId = audioId;
        _channel = channel;
    }

    public static AudioSourceBuilder Builder(string audioSrc, string channel) {
        return new AudioSourceBuilder(audioSrc, channel);
    }

    public AudioSource Build() {
        return new AudioSource(_sourceName, _audioId, _channel, _mute, _loop, _playOnAwake, _volume, _isActive);
    }

    public AudioSourceBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }

    public AudioSourceBuilder SetSourceName(string sourceName) {
        _sourceName = sourceName;
        return this;
    }
    
    public AudioSourceBuilder SetMute(bool mute) {
        _mute = mute;
        return this;
    }

    public AudioSourceBuilder SetLoop(bool loop) {
        _loop = loop;
        return this;
    }

    public AudioSourceBuilder SetPlayOnAwake(bool playOnAwake) {
        _playOnAwake = playOnAwake;
        return this;
    }

    public AudioSourceBuilder SetVolume(Volume volume) {
        _volume = volume;
        return this;
    }
}