using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractParticles : MonoBehaviour
{
    private ParticleSystem[] _particleSystems;
    private float _particleDelay = 0.2f;
    private float _particleTimer = 0f;
    void Start()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    public void Play()
    {
        _particleTimer += Time.deltaTime;
        if (_particleTimer > _particleDelay)
        {
            _particleTimer = 0;
            foreach (var ps in _particleSystems)
            {
                ps.Play();
            }
        }
    }
}
