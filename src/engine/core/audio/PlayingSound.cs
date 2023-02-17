using SDL2;

namespace Worms.engine.core.audio; 

public struct PlayingSound {
    public readonly int track;
    public readonly string channel;
    public readonly Volume currentAudioVolume;
    // We need to store this here while the audio is playing as to not have the callback be garbage collected
    private readonly SDL_mixer.ChannelFinishedDelegate _finishedCallback;

    public PlayingSound(int track, string channel, Volume currentAudioVolume, SDL_mixer.ChannelFinishedDelegate finishedCallback) {
        this.track = track;
        this.channel = channel;
        this.currentAudioVolume = currentAudioVolume;
        _finishedCallback = finishedCallback;
    }
}