using SDL2;
using Worms.engine.core.renderer.textures;

namespace Worms.engine.core.audio; 

public class AudioHandler {
    private const int MAX_TRACKS = 32;

    private static AudioHandler _self = null!;

    private readonly Dictionary<string, nint> _loadedSounds = new();
    private readonly Dictionary<long, PlayingSound> _playingSounds = new();
    private readonly AudioSettings _settings;

    private AudioHandler(AudioSettings settings, IEnumerable<AssetDeclaration> declarations) {
        if (SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.MIX_INIT_MP3) == 0) {
            throw new Exception($"Unable to init audio mix due to: {SDL_mixer.Mix_GetError()}");
        }
        if (SDL_mixer.Mix_OpenAudio(SDL_mixer.MIX_DEFAULT_FREQUENCY, SDL_mixer.MIX_DEFAULT_FORMAT, SDL_mixer.MIX_DEFAULT_CHANNELS, 2048) != 0) {
            throw new Exception($"Unable to open audio due to: {SDL_mixer.Mix_GetError()}");
        }

        if (SDL_mixer.Mix_AllocateChannels(MAX_TRACKS) != MAX_TRACKS) {
            throw new Exception($"Unable to allocate audio channels due to: {SDL_mixer.Mix_GetError()}");
        }
        
        SDL_mixer.Mix_ChannelFinished(AudioFinishCallback);
        
        foreach (AssetDeclaration declaration in declarations) {
            LoadAudio(declaration);
        }
        _settings = settings;
    }

    public static AudioHandler Init(AudioSettings settings, IEnumerable<AssetDeclaration> declarations) {
        if (_self != null) {
            throw new Exception("There can only be one audio handler!");
        }
        _self = new AudioHandler(settings, declarations);
        return _self;
    }

    public static void Play(
        string audioId,
        string channel,
        Volume audioVolume,
        long callerId,
        Action audioFinishCallback
    ) {
        if (_self._playingSounds.TryGetValue(callerId, out PlayingSound sound)) {
            SDL_mixer.Mix_Resume(sound.track);
            return;
        }

        int track = SDL_mixer.Mix_PlayChannel(-1, _self._loadedSounds[audioId], 0);
        if (track == -1) {
            throw new Exception($"Unable to play another sound as all channels are occupied, increase? {SDL_mixer.Mix_GetError()}");
        }
        SetChannelVolume(track, CalculateVolume(channel, audioVolume));
        _self._playingSounds.Add(callerId, new PlayingSound(track, channel, audioVolume, audioFinishCallback));
    }

    public static void Pause(long callerId) {
        if (!_self._playingSounds.ContainsKey(callerId)) {
            return;
        }
        SDL_mixer.Mix_Pause(_self._playingSounds[callerId].track);
    }

    public static void Stop(long callerId) {
        if (!_self._playingSounds.ContainsKey(callerId)) {
            return;
        }

        SDL_mixer.Mix_HaltChannel(_self._playingSounds[callerId].track);
    }

    public static void ChangeAudioVolume(Volume audioVolume, long callerId) {
        PlayingSound sound = _self._playingSounds[callerId];
        SetChannelVolume(sound.track, CalculateVolume(sound.channel, audioVolume));
    }

    public static void ReloadVolumeSettings() {
        foreach ((long _, PlayingSound sound) in _self._playingSounds) {
            SetChannelVolume(sound.track, CalculateVolume(sound.channel, sound.currentAudioVolume));
        }
    }
    
    public void Clean() {
        foreach ((string _, nint audioChunk) in _loadedSounds) {
            SDL_mixer.Mix_FreeChunk(audioChunk);
        }

        _playingSounds.Clear();
        _loadedSounds.Clear();
        SDL_mixer.Mix_CloseAudio();
        SDL_mixer.Mix_Quit();
    }

    private static Volume CalculateVolume(string channel, Volume audioVolume) {
        if (!_self._settings.HasChannel(channel)) {
            throw new Exception($"There exist no audio channel called: {channel}");
        }

        return _self._settings.MasterVolume * _self._settings.GetChannelVolume(channel) * audioVolume;
    }

    private static void SetChannelVolume(int channel, Volume volume) {
        SDL_mixer.Mix_Volume(channel, (int)(SDL_mixer.MIX_MAX_VOLUME * volume.Percentage / 100f));
    }
    
    private void LoadAudio(AssetDeclaration declaration) {
        nint chunk = SDL_mixer.Mix_LoadWAV(declaration.src);
        if (chunk == nint.Zero) {
            throw new ArgumentException($"Unable to load the provided sound: {declaration} due to: {SDL_mixer.Mix_GetError()}");
        }
        _loadedSounds.Add(declaration.id, chunk);
    }
    
    private static void AudioFinishCallback(int channel) {
        KeyValuePair<long, PlayingSound> entry = _self._playingSounds
            .First(entry => entry.Value.track == channel);
        _self._playingSounds.Remove(entry.Key);
        entry.Value.audioFinishCallback.Invoke();
    }
}