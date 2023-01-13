namespace Worms.engine.core.audio; 

public struct PlayingSound {
    public readonly int track;
    public readonly string channel;
    public readonly Volume currentAudioVolume;

    public PlayingSound(int track, string channel, Volume currentAudioVolume) {
        this.track = track;
        this.channel = channel;
        this.currentAudioVolume = currentAudioVolume;
    }
}