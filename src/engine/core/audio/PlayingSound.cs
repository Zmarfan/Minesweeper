using SDL2;

namespace Worms.engine.core.audio; 

public struct PlayingSound {
    public readonly int track;
    public readonly string channel;
    public readonly Volume currentAudioVolume;
    public readonly SDL_mixer.ChannelFinishedDelegate finishedCallback;

    public PlayingSound(int track, string channel, Volume currentAudioVolume, SDL_mixer.ChannelFinishedDelegate finishedCallback) {
        this.track = track;
        this.channel = channel;
        this.currentAudioVolume = currentAudioVolume;
        this.finishedCallback = finishedCallback;
    }
}