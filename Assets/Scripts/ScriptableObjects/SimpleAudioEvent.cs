using UnityEngine;

[CreateAssetMenu()]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;

    [Range(0, 1)]
    public float minVolume;
    [Range(0, 1)]
    public float maxVolume;
    [Range(0, 2)]
    public float minPitch;
    [Range(0, 2)]
    public float maxPitch;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0)
        {
            return;
        }
        
        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
    }

    public override void PlayOneShot(AudioSource source)
    {
        if (clips.Length == 0)
        {
            return;
        }

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(source.clip);
    }
}
