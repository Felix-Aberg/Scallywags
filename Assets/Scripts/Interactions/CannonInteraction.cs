using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ScallyWags 
{
    public class CannonInteraction : MonoBehaviour, IInteraction
    {
        [SerializeField] private SimpleAudioEvent _event;
        [SerializeField] private float _cannonForce = 1;
        [SerializeField] private GameObject _cannonBall;
        private ItemSpawn _spawnPos;
        private ParticleSystem[] _particleSystem;
        private AudioSourcePoolManager _audioPool;
        private ShipCondition _enemyCondition;
        private ShipCondition _shipCondition;
        private void Start()
        {
            _shipCondition = GetComponentInChildren<ShipCondition>();
            _particleSystem = GetComponentsInChildren<ParticleSystem>();
            _spawnPos = GetComponentInChildren<ItemSpawn>();
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();

            if (_spawnPos == null)
            {
                Debug.LogError("Missing spawn position script on children objects. You need to create a child game object and assign ItemSpawn script to it");
            }
        }

        public void Act()
        {
            FindTarget();
            
            // Shoot cannonball

            var rot = Quaternion.identity;
            if (_enemyCondition)
            {
                var dir = _enemyCondition.transform.position - transform.position;
                rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            }
            
            var cannonBall = Instantiate(_cannonBall, _spawnPos.transform.position, Quaternion.identity);
            cannonBall.transform.rotation = rot;
            cannonBall.GetComponent<Rigidbody>().AddForce(_spawnPos.transform.forward * _cannonForce, ForceMode.Impulse);
            foreach (var p in _particleSystem)
            {
                p.Play();   
            }
            _audioPool.PlayAudioEvent(_event, transform.position);
        }

        private void FindTarget()
        {
            var ships = FindObjectsOfType<ShipCondition>();

            foreach (var ship in ships)
            {
                if (ship.ShipType != _shipCondition?.ShipType)
                {
                    _enemyCondition = ship;
                }
            }
        }
    }
}