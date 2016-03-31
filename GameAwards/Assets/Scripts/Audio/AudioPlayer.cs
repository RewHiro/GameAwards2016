using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{

    /// <summary>
    /// internalで実装してもよかったがこのコンポーネント一つで機能させたかったのでpublic
    /// </summary>
    public AudioClip clip
    {
        get
        {
            return _audioSource.clip;
        }

        set
        {

            _audioSource.clip = value;
        }
    }

    public AudioMixerGroup audioMixerGroup
    {
        get
        {
            return _audioSource.outputAudioMixerGroup;
        }

        set
        {
            _audioSource.outputAudioMixerGroup = value;
        }
    }

    public bool loop
    {
        get
        {
            return _audioSource.loop;
        }

        set
        {
            _audioSource.loop = value;
        }
    }

    public float volume
    {
        get
        {
            return _audioSource.volume;
        }

        set
        {
            _audioSource.volume = value;
        }
    }

    public bool isFade
    {
        get
        {
            return _state != FadeState.WAIT;
        }
    }

    public bool isFadeIn
    {
        get
        {
            return _state == FadeState.IN;
        }
    }

    public bool isFadeOut
    {
        get
        {
            return _state == FadeState.OUT;
        }
    }

    public void Play(ulong delay = 0)
    {
        _audioSource.Play(delay);
    }

    public void Pause()
    {
        _audioSource.Pause();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void UnPause()
    {
        _audioSource.UnPause();
    }

    public void StartFadeIn(float fade_time = 1.0f, float max_volume = 1.0f, float current_volume = 0.0f)
    {
        if (_state == FadeState.IN) return;

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        _state = FadeState.IN;
        _audioSource.volume = current_volume;
        StopAllCoroutines();
        StartCoroutine(FadeIn(fade_time, max_volume));
    }

    public void StartFadeOut(float fade_time = 1.0f, float min_volume = 0.0f)
    {
        if (_state == FadeState.OUT) return;

        _state = FadeState.OUT;
        StopAllCoroutines();
        StartCoroutine(FadeOut(fade_time, min_volume));
    }

    //--------------------------------------------------------------

    enum FadeState
    {
        IN,
        OUT,
        WAIT
    }

    FadeState _state = FadeState.WAIT;
    AudioSource _audioSource = null;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    IEnumerator FadeIn(float fade_time = 1.0f, float max_volume = 1.0f)
    {
        float time = 0.0f;
        float min_volume = _audioSource.volume;

        while (_audioSource.volume < max_volume)
        {
            Fade(ref time, fade_time, min_volume, max_volume);
            yield return null;
        }

        _audioSource.volume = max_volume;
        _state = FadeState.WAIT;
    }

    IEnumerator FadeOut(float fade_time = 1.0f, float min_volume = 0.0f)
    {
        float time = 0.0f;
        float max_volume = _audioSource.volume;

        while (_audioSource.volume > min_volume)
        {
            Fade(ref time, fade_time, max_volume, min_volume);
            yield return null;
        }

        _audioSource.volume = min_volume;
        _state = FadeState.WAIT;
    }

    void Fade(ref float time, float fade_time, float begin_volume, float end_volume)
    {
        time += Time.deltaTime;
        _audioSource.volume = Mathf.Lerp(begin_volume, end_volume, time / fade_time);
    }
}