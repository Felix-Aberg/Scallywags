using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject particles;
    public SimpleAudioEvent _audio;
    private AudioSourcePoolManager _audioPool;

    private void Start()
    {
        _audioPool = FindObjectOfType<AudioSourcePoolManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage();

        var particleSystem = Instantiate(particles, transform.position, Quaternion.identity);
        if (particleSystem == null)
        {
            Debug.LogError("Missing particle system prefab");
        }
        particleSystem.GetComponent<ParticleSystem>().Play();
        _audioPool.PlayAudioEvent(_audio);
        gameObject.SetActive(false);
    }
}
