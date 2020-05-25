using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ScallyWags;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
        [SerializeField] private SimpleAudioEvent _event;
        [SerializeField] private GameObject _cannonBall;
        private float _maxDistance = 100;
        private float _minDistance = 1;
        private float _cannonForce = 70;
        private ItemSpawn _spawnPos;
        private ParticleSystem[] _particleSystem;
        private AudioSourcePoolManager _audioPool;
        private ShipCondition _enemyCondition;
        private ShipCondition _shipCondition;
        private List<IDamageable> _destroyable = new List<IDamageable>();
        private float angle = 15;
        private bool _shooting;


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

            if (_enemyCondition == null) return;
            
            var dist = Vector3.Distance(transform.position, _enemyCondition.transform.position);
            
            if (dist < _minDistance || dist > _maxDistance) return;
            
            Fire();
            
            foreach (var p in _particleSystem)
            {
                p.Play();   
            }
            
            _audioPool.PlayAudioEvent(_event, transform.position);
        }
    

        private void Fire()
        {
            var cannonBall = Instantiate(_cannonBall, _spawnPos.transform.position, Quaternion.identity);
            
            // Shooting destroyable items is priority. If none available shoot at the hull
            if (_destroyable.Count > 0)
            {
                var index = Random.Range(0, _destroyable.Count);

                var target = _destroyable[index].GetPos();

                Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
                var vel = BallisticVel(target, angle);
                if (vel.sqrMagnitude > 0)
                {
                    rb.velocity = vel;
                }
            }
            else
            {
                var dir = _enemyCondition.transform.position - _spawnPos.transform.position;
                Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                cannonBall.transform.rotation = rot;
                cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * _cannonForce, ForceMode.Impulse);
            }

            cannonBall.GetComponent<Explode>().Init(_shipCondition.ShipType);
        }

        private void FindTarget()
        {
            if (_enemyCondition != null) return;
            
            var ships = FindObjectsOfType<ShipCondition>();

            foreach (var ship in ships)
            {
                if (ship.ShipType != _shipCondition?.ShipType)
                {
                    _enemyCondition = ship;
                }
            }

            if (_enemyCondition == null) return;
            
            _destroyable.Clear();
            var targets = _enemyCondition.GetComponentsInChildren<IDamageable>();

            foreach (var target in targets)
            {
                var railing = target as RailingInteraction;
                if (railing && railing.Broken)
                {
                    continue;
                }
                _destroyable.Add(target);
            }
        }
        
        private Vector3 BallisticVel(Vector3 target, float angle) {
            var dir = target - _spawnPos.transform.position;  // get target direction
            var h = dir.y;  // get height difference
            dir.y = 0;  // retain only the horizontal direction
            var dist = dir.magnitude ;  // get horizontal distance
            var a = angle * Mathf.Deg2Rad;  // convert angle to radians
            dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
            dist += h / Mathf.Tan(a);  // correct for small height differences
            // calculate the velocity magnitude
            var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return vel * dir.normalized;
        }
}
