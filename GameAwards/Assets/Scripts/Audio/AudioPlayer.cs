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
            return _fadeState.isFade;
        }
    }

    public bool isFadeIn
    {
        get
        {
            return _fadeState.isFadeIn;
        }
    }

    public bool isFadeOut
    {
        get
        {
            return _fadeState.isFadeOut;
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
        if (_fadeState.isFadeIn) return;

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        _fadeState.state = FadeState.State.IN;
        _audioSource.volume = current_volume;
        StopAllCoroutines();
        StartCoroutine(FadeIn(fade_time, max_volume));
    }

    public void StartFadeOut(float fade_time = 1.0f, float min_volume = 0.0f)
    {
        if (_fadeState.isFadeOut) return;

        _fadeState.state = FadeState.State.OUT;
        StopAllCoroutines();
        StartCoroutine(FadeOut(fade_time, min_volume));
    }

    //--------------------------------------------------------------

    FadeState _fadeState = new FadeState();
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
        _fadeState.state = FadeState.State.WAIT;
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
        _fadeState.state = FadeState.State.WAIT;
    }

    void Fade(ref float time, float fade_time, float begin_value, float end_value)
    {
        time += Time.deltaTime;
        _audioSource.volume = Mathf.Lerp(begin_value, end_value, time / fade_time);
    }
}