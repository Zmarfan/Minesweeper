using SDL2;

namespace Worms.engine.core.audio; 

public class AudioHandler {
    private const int MAX_TRACKS = 32;

    private static AudioHandler _self = null!;

    private readonly Dictionary<string, IntPtr> _loadedSounds = new();
    private readonly Dictionary<string, PlayingSound> _playingSounds = new();
    private readonly AudioSettings _settings;

    private AudioHandler(AudioSettings settings) {
        SDL_mixer.Mix_Init(SDL_mixer.MIX_InitFlags.MIX_INIT_MP3);
        if (SDL_mixer.Mix_OpenAudio(SDL_mixer.MIX_DEFAULT_FREQUENCY, SDL_mixer.MIX_DEFAULT_FORMAT, SDL_mixer.MIX_DEFAULT_CHANNELS, 2048) == -1) {
            throw new Exception();
        }

        SDL_mixer.Mix_AllocateChannels(MAX_TRACKS);
        _settings = settings;
    }

    public static void Init(AudioSettings settings) {
        if (_self != null) {
            throw new Exception("There can only be one audio handler!");
        }
        _self = new AudioHandler(settings);
    }

    public static void Play(
        string audioSrc,
        string channel,
        Volume audioVolume,
        string callerId,
        Action audioFinishCallback
    ) {
        if (_self._playingSounds.TryGetValue(callerId, out PlayingSound sound)) {
            SDL_mixer.Mix_Resume(sound.track);
            return;
        }
        
        if (!_self._loadedSounds.ContainsKey(audioSrc)) {
            LoadAudio(audioSrc);
        }

        int track = SDL_mixer.Mix_PlayChannel(-1, _self._loadedSounds[audioSrc], 0);
        if (track == -1) {
            throw new Exception($"Unable to play another sound as all channels are occupied, increase? {SDL_mixer.Mix_GetError()}");
        }
        SetChannelVolume(track, CalculateVolume(channel, audioVolume));
        SDL_mixer.Mix_ChannelFinished(_ => {
            _self._playingSounds.Remove(callerId);
            audioFinishCallback.Invoke();
        });
        _self._playingSounds.Add(callerId, new PlayingSound(track, channel, audioVolume));
    }

    public static void Pause(string callerId) {
        if (!_self._playingSounds.ContainsKey(callerId)) {
            return;
        }
        SDL_mixer.Mix_Pause(_self._playingSounds[callerId].track);
    }

    public static void Stop(string callerId) {
        if (!_self._playingSounds.ContainsKey(callerId)) {
            return;
        }

        SDL_mixer.Mix_HaltChannel(_self._playingSounds[callerId].track);
    }

    public static void ReloadVolumeSettings() {
        foreach ((string _, PlayingSound sound) in _self._playingSounds) {
            SetChannelVolume(sound.track, CalculateVolume(sound.channel, sound.currentAudioVolume));
        }
    }
    
    public static void ChangeAudioVolume(Volume audioVolume, string callerId) {
        PlayingSound sound = _self._playingSounds[callerId];
        SetChannelVolume(sound.track, CalculateVolume(sound.channel, audioVolume));
    }

    public static void Clean() {
        foreach ((string _, IntPtr audioChunk) in _self._loadedSounds) {
            SDL_mixer.Mix_FreeChunk(audioChunk);
        }

        _self._playingSounds.Clear();
        _self._loadedSounds.Clear();
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
        Console.WriteLine(volume);
        SDL_mixer.Mix_Volume(channel, (int)(SDL_mixer.MIX_MAX_VOLUME * volume.Percentage / 100f));
    }
    
    private static void LoadAudio(string audioSrc) {
        IntPtr chunk = SDL_mixer.Mix_LoadWAV(audioSrc);
        if (chunk == IntPtr.Zero) {
            throw new ArgumentException($"Unable to load the provided sound: {audioSrc} due to: {SDL_mixer.Mix_GetError()}");
        }
        _self._loadedSounds.Add(audioSrc, chunk);
    }
}