using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    const string BGM_NAME = "Bgm";
    const string SE_NAME = "Se";
    const string VOICE_NAME = "Voice";


    public AudioPlayer GetBgm(uint index)
    {
        return GetAudioPlayer(index, _bgms);
    }

    public AudioPlayer GetSe(uint index)
    {
        return GetAudioPlayer(index, _ses);
    }

    public AudioPlayer GetVoice(uint index)
    {
        return GetAudioPlayer(index, _voices);
    }

    public AudioPlayer PlayBgm(uint index)
    {
        var audioPlayer = GetBgm(index);
        if (audioPlayer == null) throw new NullReferenceException("bgm" + index + "はnullです");

        audioPlayer.Play();

        return audioPlayer;
    }

    public AudioPlayer PlaySe(uint index)
    {
        var audioPlayer = GetSe(index);
        if (audioPlayer == null) throw new NullReferenceException("se" + index + "はnullです");

        audioPlayer.Play();

        return audioPlayer;
    }

    public AudioPlayer PlayVoice(uint index)
    {
        var audioPlayer = GetVoice(index);
        if (audioPlayer == null) throw new NullReferenceException("voice" + index + "はnullです");

        audioPlayer.Play();

        return audioPlayer;
    }

    public AudioPlayer StopBgm(uint index)
    {
        var audioPlayer = GetBgm(index);
        if (audioPlayer == null) throw new NullReferenceException("bgm" + index + "はnullです");

        audioPlayer.Stop();

        return audioPlayer;
    }

    public AudioPlayer StopSe(uint index)
    {
        var audioPlayer = GetSe(index);
        if (audioPlayer == null) throw new NullReferenceException("se" + index + "はnullです");

        audioPlayer.Stop();

        return audioPlayer;
    }

    public AudioPlayer StopVoice(uint index)
    {
        var audioPlayer = GetVoice(index);
        if (audioPlayer == null) throw new NullReferenceException("voice" + index + "はnullです");

        audioPlayer.Stop();

        return audioPlayer;
    }

    public AudioManager StopAll()
    {
        StopBgm();
        StopSe();
        StopVoice();
        return this;
    }

    public AudioManager StopBgm()
    {
        _bgms.ExecuteAction(audio => audio.Stop());
        return this;
    }

    public AudioManager StopSe()
    {
        _ses.ExecuteAction(audio => audio.Stop());
        return this;
    }

    public AudioManager StopVoice()
    {
        _voices.ExecuteAction(audio => audio.Stop());
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dB">20~-80(0がAudioSourceのVolumeの1に当たる)</param>
    public AudioManager ChangeMasterVolume(float dB)
    {
        _audioMixerGroup[0].audioMixer.SetFloat("MasterVolume", dB);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dB">20~-80(0がAudioSourceのVolumeの1に当たる)</param>
    public AudioManager ChangeBgmVolume(float dB)
    {
        _audioMixerGroup[1].audioMixer.SetFloat("BgmVolume", dB);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dB">20~-80(0がAudioSourceのVolumeの1に当たる)</param>
    public AudioManager ChangeSeVolume(float dB)
    {
        _audioMixerGroup[2].audioMixer.SetFloat("SeVolume", dB);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dB">20~-80(0がAudioSourceのVolumeの1に当たる)</param>
    public AudioManager ChangeVoiceVolume(float dB)
    {
        _audioMixerGroup[3].audioMixer.SetFloat("VoiceVolume", dB);
        return this;
    }

    //--------------------------------------------------------

    List<AudioPlayer> _bgms = new List<AudioPlayer>();
    List<AudioPlayer> _ses = new List<AudioPlayer>();
    List<AudioPlayer> _voices = new List<AudioPlayer>();
    List<AudioMixerGroup> _audioMixerGroup = new List<AudioMixerGroup>();


    override protected void Awake()
    {
        base.Awake();
        var audioPlayerPrefab = Resources.Load<GameObject>(PathInfo.pathOfSound + "Prefabs/AudioPlayer");

        var bgms = Resources.LoadAll<AudioClip>(PathInfo.pathOfBgm);
        var ses = Resources.LoadAll<AudioClip>(PathInfo.pathOfSe);
        var voices = Resources.LoadAll<AudioClip>(PathInfo.pathOfVoice);

        var transform = gameObject.transform;

        var audioMixer = Resources.Load<AudioMixer>(PathInfo.pathOfSound + "AudioMixer");
        var audioMixerGroup = audioMixer.FindMatchingGroups(string.Empty);

        _audioMixerGroup.AddRange(audioMixerGroup);

        Create(audioPlayerPrefab, bgms, _bgms, transform.FindChild(BGM_NAME), audioMixerGroup[1], true);
        Create(audioPlayerPrefab, ses, _ses, transform.FindChild(SE_NAME), audioMixerGroup[2]);
        Create(audioPlayerPrefab, voices, _voices, transform.FindChild(VOICE_NAME), audioMixerGroup[3]);
    }

    void Create
        (
        GameObject prefab,
        IEnumerable<AudioClip> audios,
        List<AudioPlayer> list,
        Transform parent,
        AudioMixerGroup audio_mixer_group,
        bool loop = false
        )
    {
        foreach (var audio in audios)
        {
            var audioPlayerObject = Instantiate(prefab);
            audioPlayerObject.transform.SetParent(parent);
            audioPlayerObject.name = audio.name;

            var audioPlayer = audioPlayerObject.GetComponent<AudioPlayer>();
            audioPlayer.clip = audio;
            audioPlayer.loop = loop;
            audioPlayer.audioMixerGroup = audio_mixer_group;

            list.Add(audioPlayer);
        }
    }

    AudioPlayer GetAudioPlayer(uint index, List<AudioPlayer>audios)
    {
        if (audios.Count <= index) throw new ArgumentOutOfRangeException("無効なindexです");
        return audios[(int)index];
    }
}