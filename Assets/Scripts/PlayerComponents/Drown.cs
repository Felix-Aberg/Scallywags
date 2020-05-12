using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drown : MonoBehaviour
{
    [SerializeField] private SimpleAudioEvent _drowning;

    private AudioSource _audioSource;
    private bool _drowned;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = _drowning.MixerGroup;
    }

    public void Tick()
    {
        if (transform.position.y > 0)
        {
            _drowned = false;
        }
        
        if (_drowned) return;
        
        if (transform.position.y < -5)
        {
            _drowned = true;
            _drowning.Play(_audioSource);
        }
    }
}
