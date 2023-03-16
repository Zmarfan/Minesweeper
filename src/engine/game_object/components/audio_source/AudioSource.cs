using Worms.engine.core.audio;
using Worms.engine.game_object.scripts;

namespace Worms.engine.game_object.components.audio_source; 

public class AudioSource : Script {
    private static long UNIQUE_ID = long.MinValue;
    
    public string audioId;
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

    public bool IsPlaying { get; private set; }
    public bool IsPaused { get; private set; }

    private bool _mute;
    private Volume _volume;
    private readonly long _uniqueId = UNIQUE_ID++;

    public AudioSource(
        string audioId,
        string channel,
        bool mute,
        bool loop,
        bool playOnAwake,
        Volume volume,
        bool isActive,
        string name
    ) : base(isActive, name) {
        this.audioId = audioId;
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
        AudioHandler.Play(audioId, channel, GetPlayVolume(), _uniqueId, AudioFinished);
        IsPlaying = true;
        IsPaused = false;
    }
    
    public void Pause() {
        AudioHandler.Pause(_uniqueId);
        IsPaused = true;
    }

    public void Stop() {
        AudioHandler.Stop(_uniqueId);
        IsPlaying = false;
        IsPaused = false;
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
        bool playAgain = IsPlaying && !IsPaused;
        IsPlaying = false;
        IsPaused = false;
        if (loop && playAgain) {
            Play();
        }
    }

    public override void OnDestroy() {
        Stop();
    }
}