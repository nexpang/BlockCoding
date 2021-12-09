using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioBox _audioBox;
    public AudioSource _bgmSource;
    public AudioSource _sfxSource;

    public static AudioBox audioBox;
    public static AudioSource bgmSource;
    public static AudioSource sfxSource;

    private void Awake()
    {
        audioBox = _audioBox;
        bgmSource = _bgmSource;
        sfxSource = _sfxSource;

        RefreshAudioSourceStatus();
    }

    public static void PlayBGM(AudioClip bgm, float volume = 1)
    {
        bgmSource.clip = bgm;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public static void PlaySFX(AudioClip sfx, float volume = 1)
    {
        sfxSource.PlayOneShot(sfx, volume);
    }

    public static void SetMute(AudioSource source, bool value)
    {
        source.mute = value;
        SecurityPlayerPrefs.SetBool("option.mute.bgm", bgmSource.mute);
        SecurityPlayerPrefs.SetBool("option.mute.sfx", sfxSource.mute);
    }

    private static void RefreshAudioSourceStatus()
    {
        bgmSource.mute = SecurityPlayerPrefs.GetBool("option.mute.bgm", false);
        sfxSource.mute = SecurityPlayerPrefs.GetBool("option.mute.sfx", false);
    }
}
