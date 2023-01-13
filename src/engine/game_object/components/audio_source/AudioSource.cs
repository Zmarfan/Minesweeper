using Worms.engine.core.audio;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.audio_source; 

public class AudioSource : Script {
    private static readonly string ROOT_DIRECTORY = Directory.GetCurrentDirectory();
    
    public string audioSrc;
    public string channel;
    public bool Mute {
        get => _mute;
        set {
            VolumeChanged();
            _mute = value;
        }
    }
    public bool loop;
    public bool playOnAwake;
    public Volume Volume {
        get => _volume;
        set {
            VolumeChanged();
            _volume = value;
        }
    }

    public bool IsPlaying { get; private set; } = false;
    public bool IsPaused { get; private set; } = false;

    private bool _mute;
    private Volume _volume;
    private readonly string _uniqueId = Guid.NewGuid().ToString();

    public AudioSource(
        string audioSrc,
        string channel,
        bool mute,
        bool loop,
        bool playOnAwake,
        Volume volume,
        bool isActive
    ) : base(isActive) {
        this.audioSrc = audioSrc;
        this.channel = channel;
        _mute = mute;
        this.loop = loop;
        this.playOnAwake = playOnAwake;
        _volume = volume;
    }

    public override void Awake() {
        if (playOnAwake) {
            Play();
        }
    }

    public void Play() {
        AudioHandler.Play($"{ROOT_DIRECTORY}\\{audioSrc}", channel, GetPlayVolume(), _uniqueId, AudioFinished);
        IsPlaying = true;
        IsPaused = false;
    }
    
    public void Pause() {
        AudioHandler.Pause(_uniqueId);
        IsPaused = true;
    }

    public void Stop() {
        AudioHandler.Stop(_uniqueId);
        AudioFinished();
    }

    public void Restart() {
        Stop();
        Play();
    }

    private void VolumeChanged() {
        AudioHandler.ChangeAudioVolume(GetPlayVolume(), _uniqueId);
    }

    private Volume GetPlayVolume() {
        return _mute ? Volume.Zero() : _volume;
    }
    
    private void AudioFinished() {
        IsPlaying = false;
        IsPaused = false;
        if (loop) {
            Play();
        }
    }
}