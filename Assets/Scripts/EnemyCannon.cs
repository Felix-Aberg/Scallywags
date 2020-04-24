using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ScallyWags;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
        [SerializeField] private SimpleAudioEvent _event;
        [SerializeField] private GameObject _cannonBall;
        private float _cannonForce = 70;
        private ItemSpawn _spawnPos;
        private ParticleSystem[] _particleSystem;
        private AudioSourcePoolManager _audioPool;
        private ShipCondition _enemyCondition;
        private ShipCondition _shipCondition;
        private List<Destructable> _destroyable = new List<Destructable>();
        private float angle = 15;

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
            if (_enemyCondition)
            {
                var dist = Vector3.Distance(_enemyCondition.transform.position, transform.position);
                if (dist < 25)
                {
                    return;
                }
            }
            
            FindTarget();
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
                Vector3 target = _destroyable[index].transform.position;
                Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
                rb.velocity = BallisticVel(target, angle);
            }
            else
            {
                var dir = _enemyCondition.transform.position - _spawnPos.transform.position;
                Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                cannonBall.transform.rotation = rot;
                cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * _cannonForce, ForceMode.Impulse);
            }
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
            
            var targets = _enemyCondition.GetComponentsInChildren<Destructable>();

            foreach (var target in targets)
            {
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
