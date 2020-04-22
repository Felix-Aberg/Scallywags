using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ScallyWags 
{
    public class CannonInteraction : MonoBehaviour, IInteraction
    {
        [SerializeField] private SimpleAudioEvent _event;
        [SerializeField] private GameObject _cannonBall;
        private float _cannonForce = 70;
        private ItemSpawn _spawnPos;
        private ParticleSystem[] _particleSystem;
        private AudioSourcePoolManager _audioPool;
        private ShipCondition _enemyCondition;
        private ShipCondition _shipCondition;
        private void Start()
        {
            _shipCondition = GetComponentInParent<ShipCondition>();
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
            
            EventManager.TriggerEvent("IntroDone", new EventManager.EventMessage(null));

            var rot = _spawnPos.transform.rotation;
            var cannonBall = Instantiate(_cannonBall, _spawnPos.transform.position, rot);
            
            if (_enemyCondition)
            {
                var dir = _enemyCondition.transform.position - _spawnPos.transform.position;
                rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                cannonBall.transform.rotation = rot;
            }
            
            cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * _cannonForce, ForceMode.Impulse);
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