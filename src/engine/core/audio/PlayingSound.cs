using SDL2;

namespace Worms.engine.core.audio; 

public struct PlayingSound {
    public readonly int track;
    public readonly string channel;
    public readonly Volume currentAudioVolume;
    public readonly Action audioFinishCallback;

    public PlayingSound(int track, string channel, Volume currentAudioVolume, Action audioFinishCallback) {
        this.track = track;
        this.channel = channel;
        this.currentAudioVolume = currentAudioVolume;
        this.audioFinishCallback = audioFinishCallback;
    }
}