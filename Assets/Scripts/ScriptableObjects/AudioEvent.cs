using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class AudioEvent : ScriptableObject
{
    public abstract void Play(AudioSource source);
    public abstract void PlayOneShot(AudioSource source);
    public AudioMixerGroup MixerGroup;
}
