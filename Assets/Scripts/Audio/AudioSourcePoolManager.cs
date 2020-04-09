using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePoolManager : MonoBehaviour
{
    private GameObject prefab;
    private AudioSourcePool _pool;

    void Start()
    {
        prefab = new GameObject();
        prefab.AddComponent<AudioSource>();
        _pool = new AudioSourcePool(prefab.GetComponent<AudioSource>());
    }

    public AudioSource GetAudioSource()
    {
        return _pool.GetAudioSource();
    }

    public void PlayAudioEvent(AudioEvent audioEvent)
    {
        _pool.Play(audioEvent);
    }

    public void PlayAudioEvent(AudioEvent audioEvent, Vector3 pos)
    {
        _pool.Play(audioEvent, pos);
    }
}
