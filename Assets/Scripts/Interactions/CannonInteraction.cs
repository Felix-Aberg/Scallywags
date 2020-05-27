using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ScallyWags 
{
    public class CannonInteraction : MonoBehaviour, IInteraction
    {
        [SerializeField] private SimpleAudioEvent _event;
        [SerializeField] private GameObject _cannonBall;
        private float _cannonForce = 500;
        private ItemSpawn _spawnPos;
        private ParticleSystem[] _particleSystem;
        private AudioSourcePoolManager _audioPool;
        private CannonTarget _cannonTarget;
        private ShipCondition _shipCondition;
        private Animator _animator;
        private void Start()
        {
            _shipCondition = GetComponentInParent<ShipCondition>();
            _particleSystem = GetComponentsInChildren<ParticleSystem>();
            _spawnPos = GetComponentInChildren<ItemSpawn>();
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();
            _animator = GetComponentInChildren<Animator>();

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
            var cannonBall = Instantiate(_cannonBall, _spawnPos.transform.position, Quaternion.identity);
            cannonBall.GetComponent<Explode>().Init(_shipCondition.ShipType);
            
            if (_cannonTarget)
            {
                var dir = _cannonTarget.transform.position - _spawnPos.transform.position;
                rot = Quaternion.LookRotation(dir.normalized, cannonBall.transform.up);
                cannonBall.transform.localRotation = rot;
            }
            
            cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * _cannonForce, ForceMode.Impulse);
            foreach (var p in _particleSystem)
            {
                p.Play();   
            }
            _audioPool.PlayAudioEvent(_event, transform.position);
            
            _animator.SetTrigger("Shoot");
        }

        private void FindTarget()
        {
            _cannonTarget = FindObjectOfType<CannonTarget>();
        }
    }
}