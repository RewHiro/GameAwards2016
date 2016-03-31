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

    public void StartFadeIn()
    {
        if (_state != FadeState.WAIT) return;
        _state = FadeState.IN;
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        if (_state != FadeState.WAIT) return;
        _state = FadeState.OUT;
        StartCoroutine(FadeOut());
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
        float TIME = fade_time;

        while (_audioSource.volume > 0)
        {
            fade_time += -Time.deltaTime;
            _audioSource.volume = ((TIME - fade_time) / TIME) * max_volume;
            yield return null;
        }
        _state = FadeState.WAIT;
    }

    IEnumerator FadeOut(float fade_time = 1.0f, float min_volume = 1.0f)
    {
        float TIME = fade_time;

        while (_audioSource.volume < 1)
        {
            fade_time += -Time.deltaTime;
            _audioSource.volume = (fade_time / TIME) * min_volume;
            yield return null;
        }
        _state = FadeState.WAIT;
    }
}