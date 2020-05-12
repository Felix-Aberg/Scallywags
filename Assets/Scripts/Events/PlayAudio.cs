using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScallyWags;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
    [SerializeField]
    private string eventName;
  //  private AudioSource audioSource;
    public SimpleAudioEvent sound;
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    void OnEnable()
    {
        EventManager.StartListening(eventName, PlaySound);
    }

    void OnDisable() {
        EventManager.StopListening(eventName, PlaySound);
    }

    private void PlaySound(EventManager.EventMessage msg)
    {
        _audioSource.outputAudioMixerGroup = sound.MixerGroup;
        sound.Play(_audioSource);
    }
}
