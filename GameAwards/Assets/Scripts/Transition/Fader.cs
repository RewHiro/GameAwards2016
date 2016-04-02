using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{

    const string RANGE_NAME = "_Range";

    [SerializeField]
    float time = 1.0f;

    public float range
    {
        get
        {
            return _material.GetFloat(RANGE_NAME);
        }

        set
        {
            _material.SetFloat(RANGE_NAME, value);
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

    public Fader StartFadeIn(float fade_time = 1.0f)
    {
        if (_fadeState.isFadeIn) return this;

        _fadeState.state = FadeState.State.IN;
        StopAllCoroutines();
        StartCoroutine(FadeIn(fade_time));

        return this;
    }

    public Fader StartFadeOut(float fade_time = 1.0f)
    {
        if (_fadeState.isFadeOut) return this;

        _fadeState.state = FadeState.State.OUT;
        StopAllCoroutines();
        StartCoroutine(FadeOut(fade_time));

        return this;
    }

    //--------------------------------------------------------------

    FadeState _fadeState = new FadeState();
    Material _material = null;

    void Awake()
    {
        var graphic = GetComponent<Graphic>();
        var render = GetComponent<Renderer>();

        if (graphic != null)
        {
            graphic.material = Instantiate(graphic.material);
            _material = graphic.material;
        }

        if (render != null)
        {
            _material = render.material;
        }
    }

    void Start()
    {
        StartFadeOut(time);
    }

    IEnumerator FadeIn(float fade_time = 1.0f)
    {
        float time = 0.0f;
        var min = _material.GetFloat(RANGE_NAME);
        var max = 2.0f;

        while (_material.GetFloat(RANGE_NAME) < max)
        {
            Fade(ref time, fade_time, min, max);
            yield return null;
        }

        _material.SetFloat(RANGE_NAME, max);
        _fadeState.state = FadeState.State.WAIT;
    }

    IEnumerator FadeOut(float fade_time = 1.0f)
    {
        float time = 0.0f;
        float min = 0.0f;
        float max = _material.GetFloat(RANGE_NAME);

        while (_material.GetFloat(RANGE_NAME) > min)
        {
            Fade(ref time, fade_time, max, min);
            yield return null;
        }

        _material.SetFloat(RANGE_NAME, min);
        _fadeState.state = FadeState.State.WAIT;
    }

    void Fade(ref float time, float fade_time, float begin_value, float end_value)
    {
        time += Time.deltaTime;
        var range = Mathf.Lerp(begin_value, end_value, time / fade_time);
        _material.SetFloat(RANGE_NAME, range);
    }
}
