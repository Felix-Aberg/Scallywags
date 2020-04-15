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
        private ParticleSystem _particleSystem;
        private AudioSourcePoolManager _audioPool;
        private ShipCondition _enemyCondition;
        private ShipCondition _ship;
        private void Start()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _spawnPos = GetComponentInChildren<ItemSpawn>();
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();
            _ship = GetComponentInParent<ShipCondition>();

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
                rot = Quaternion.LookRotation(_enemyCondition.transform.position);
            }
            var cannonBall = Instantiate(_cannonBall, _spawnPos.transform.position, rot);
            cannonBall.GetComponent<Rigidbody>().AddForce(_spawnPos.transform.forward * _cannonForce, ForceMode.Impulse);
            _particleSystem.Play();
            _audioPool.PlayAudioEvent(_event, transform.position);
        }

        private void FindTarget()
        {
            var ships = FindObjectsOfType<ShipCondition>();

            foreach (var ship in ships)
            {
                if (ship != _ship)
                {
                    _enemyCondition = ship;
                }
            }
        }
    }
}