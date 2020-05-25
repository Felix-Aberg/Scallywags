using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;
    public SimpleAudioEvent music;
    private AudioSource audioSource;

    // index 0 = menu track
    // index 1 = game track
    // index 2 = lose track
    // index 3 = win track
    
    // TODO create Kraken adaptive thingy

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void OnEnable()
    {
        EventManager.StartListening("PlayMenuMusic", PlayMenuMusic);
        EventManager.StartListening("PlayBattleMusic", PlayBattleMusic);
        EventManager.StartListening("PlayWinMusic", PlayWinMusic);
        EventManager.StartListening("PlayLoseMusic", PlayLoseMusic);
        EventManager.StartListening("StopMusic", StopMusic);
    }

    void OnDisable()
    {
        EventManager.StopListening("PlayMenuMusic", PlayMenuMusic);
        EventManager.StopListening("PlayBattleMusic", PlayBattleMusic);
        EventManager.StopListening("PlayWinMusic", PlayWinMusic);
        EventManager.StopListening("PlayLoseMusic", PlayLoseMusic);
        EventManager.StopListening("StopMusic", StopMusic);
    }

    private void PlayMenuMusic(EventManager.EventMessage msg)
    {
        if (audioSource.clip == music.clips[0]) return;
        audioSource.Stop();
        audioSource.clip = music.clips[0];
        audioSource.loop = true;
        audioSource.Play();
    }
    
    private void PlayBattleMusic(EventManager.EventMessage msg)
    {
        if (audioSource.clip == music.clips[1]) return;
        audioSource.Stop();
        audioSource.clip = music.clips[1];
        audioSource.Play();
    }
    
    private void PlayLoseMusic(EventManager.EventMessage msg)
    {
        if (audioSource.clip == music.clips[2]) return;
        audioSource.Stop();
        audioSource.clip = music.clips[2];
        audioSource.Play();
    }

    private void PlayWinMusic(EventManager.EventMessage msg)
    {
        if (audioSource.clip == music.clips[3]) return;
        audioSource.Stop();
        audioSource.clip = music.clips[3];
        audioSource.Play();
    }

    private void StopMusic(EventManager.EventMessage msg)
    {
        audioSource.Stop();
    }
}
}
