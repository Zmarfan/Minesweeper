using GameEngine.engine.core.audio;

namespace GameEngine.engine.game_object.components.audio_source;

public class AudioSourceBuilder {
    private bool _isActive = true;
    private string _name = "audioSource";
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
        return new AudioSource(_audioId, _channel, _mute, _loop, _playOnAwake, _volume, _isActive, _name);
    }

    public AudioSourceBuilder SetIsActive(bool isActive) {
        _isActive = isActive;
        return this;
    }

    public AudioSourceBuilder SetName(string name) {
        _name = name;
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