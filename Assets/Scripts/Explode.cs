using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace ScallyWags
{
    public class Explode : MonoBehaviour
    {
        public GameObject holePrefab;
        public GameObject firePrefab;
        public GameObject particles;
        [SerializeField] private bool causesFire;
        [SerializeField] private bool createsHoles;
        [SerializeField] private bool causesHullDamage;
        public SimpleAudioEvent _audio;
        private AudioSourcePoolManager _audioPool;
        private HoleSpawn[] _holeSpawns;

        private void Start()
        {
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();
            _holeSpawns = FindObjectsOfType<HoleSpawn>();
        }

        private void OnCollisionEnter(Collision other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable == null)
            {
                damageable = other.gameObject.GetComponentInParent<IDamageable>();
            }

            damageable?.TakeDamage();

            var particleSystem = Instantiate(particles, transform.position, Quaternion.identity);
            if (particleSystem == null)
            {
                Debug.LogError("Missing particle system prefab");
            }
            particleSystem.GetComponent<ParticleSystem>().Play();
            _audioPool.PlayAudioEvent(_audio, transform.position);

            var ship = other.gameObject.GetComponentInParent<ShipCondition>();
            
            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
            {
                if (hit.collider.gameObject.GetComponentInParent<ShipCondition>())
                {
                    var pos = hit.point;
                    if (createsHoles)
                    {
                        var hole = Instantiate(holePrefab, pos, Quaternion.identity);
                        hole.transform.SetParent(hit.transform);
                        hole.transform.localRotation = Quaternion.identity;
                        ship?.TakeDamage();
                    }
                    if (causesFire)
                    {
                        var fire = Instantiate(firePrefab, pos, Quaternion.identity);
                        fire.transform.SetParent(hit.transform);
                        fire.transform.localRotation = Quaternion.identity;
                    }
                }
            }
            
            Destroy(gameObject);
        }
    }
}