using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AudioBox", order = 1)]
public class AudioBox : ScriptableObject
{
    [Header("BGM")]
    public AudioClip BGM_title;

    [Header("SFX")]
    public AudioClip SFX_gameStart;
    public AudioClip SFX_connect;
    public AudioClip SFX_reset;
    public AudioClip SFX_playerDead;
    public AudioClip SFX_buttonClick;
    public AudioClip SFX_stageClear;
    public AudioClip SFX_fingerSnap;
}