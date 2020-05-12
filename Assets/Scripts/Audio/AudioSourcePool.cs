using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool : PollingPool<AudioSource>
{
    public AudioSourcePool(AudioSource prefab) : base(prefab)
    {
    }

    public AudioSource GetAudioSource()
    {
        return Get();
    }

    protected override bool IsActive(AudioSource component)
    {
        return component.isPlaying;
    }

    public void Play(AudioEvent simpleAudioEvent)
    {
        var source = Get();
        source.outputAudioMixerGroup = simpleAudioEvent.MixerGroup;
        simpleAudioEvent.Play(source);
    }

    public void Play(AudioEvent simpleAudioEvent, Vector3 pos)
    {
        var source = Get();
        source.transform.position = pos;
        source.spatialBlend = 0.5f;
        source.outputAudioMixerGroup = simpleAudioEvent.MixerGroup;
        simpleAudioEvent.Play(source);
    }
}
