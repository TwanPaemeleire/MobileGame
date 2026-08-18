using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class MusicEntry
{
    public AudioClip AudioClip;
    public float Volume;
    [HideInInspector] public AudioSource AudioSource;
}

public class SoundHandler : MonoSingleton<SoundHandler>
{
    [SerializeField] private SerializedDictionary<string, AudioClip> _sfxLibrary;
    [SerializeField] private SerializedDictionary<string, MusicEntry> _musicSourcesLibrary;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private float _musicFadeDuration = 0.5f;

    private string _currentMusicKey = "";

    private Transform _musicSourcesParent;

    protected override void Init()
    {
        _musicSourcesParent = new GameObject("MusicSources").GetComponent<Transform>();
        _musicSourcesParent.SetParent(this.transform);
        foreach(var keyValuePair in _musicSourcesLibrary)
        {
            GameObject go = new GameObject(keyValuePair.Value.AudioClip.name + "AudioSource");
            go.GetComponent<Transform>().SetParent(_musicSourcesParent);
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = 0.0f;
            audioSource.playOnAwake = false;
            audioSource.clip = keyValuePair.Value.AudioClip;
            keyValuePair.Value.AudioSource = audioSource;
        }
    }

    public void PlaySFX(string sfxKey, float volumeScale)
    {
        if(!_sfxLibrary.ContainsKey(sfxKey))
        {
            Debug.LogError("Trying to play an SFX that has not been added to the SFX library");
            return;
        }
        _sfxAudioSource.PlayOneShot(_sfxLibrary[sfxKey], volumeScale);
    }

    public void PlayMusic(string musicKey)
    {
        if(!_musicSourcesLibrary.ContainsKey(musicKey))
        {
            Debug.LogError("Trying to play music that has not been added to the music library");
            return;
        }

        if(_currentMusicKey != string.Empty)
        {
            StartCoroutine(FadeMusic(_currentMusicKey, false));
        }
        _currentMusicKey = musicKey;
        StartCoroutine(FadeMusic(_currentMusicKey, true));
    }

    public void StopMusic()
    {
        if (_currentMusicKey == string.Empty) return;
        StartCoroutine(FadeMusic(_currentMusicKey, false));
    }

    private IEnumerator FadeMusic(string musicKey, bool isFadeIn)
    {
        if (isFadeIn) _musicSourcesLibrary[musicKey].AudioSource.Play();
        float time = 0.0f;
        float startVolume = (isFadeIn) ? 0.0f : _musicSourcesLibrary[musicKey].Volume;
        float endVolume = (isFadeIn) ? _musicSourcesLibrary[musicKey].Volume : 0.0f;
        while (time < _musicFadeDuration)
        {
            time += Time.deltaTime;
            _musicSourcesLibrary[musicKey].AudioSource.volume = Mathf.Lerp(startVolume, endVolume, time);
            yield return null;
        }
        _musicSourcesLibrary[musicKey].AudioSource.volume = endVolume;
        if (!isFadeIn) _musicSourcesLibrary[musicKey].AudioSource.Stop();
    }
}