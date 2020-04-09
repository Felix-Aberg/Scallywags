using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ScallyWags 
{
    [RequireComponent(typeof(ParticleSystem))]
    public class CannonInteraction : MonoBehaviour, IInteraction
    {
        [SerializeField] private SimpleAudioEvent _event;
        [SerializeField] private float _cannonForce = 1;
        [SerializeField] private GameObject _cannonBall;
        private ItemSpawn _spawnPos;
        private ParticleSystem _particleSystem;
        private AudioSourcePoolManager _audioPool;
        private void Start()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _spawnPos = GetComponentInChildren<ItemSpawn>();
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();

            if (_spawnPos == null)
            {
                Debug.LogError("Missing spawn position script on children objects. You need to create a child game object and assign ItemSpawn script to it");
            }
        }

        public void Act()
        {
            // Shoot cannonball
            var cannonBall = Instantiate(_cannonBall, _spawnPos.transform.position, Quaternion.identity);
            cannonBall.GetComponent<Rigidbody>().AddForce(_spawnPos.transform.forward * _cannonForce, ForceMode.Impulse);
            _particleSystem.Play();
            _audioPool.PlayAudioEvent(_event, transform.position);
        }
    }
}