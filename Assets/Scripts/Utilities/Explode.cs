using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace ScallyWags
{
    public class Explode : MonoBehaviour
    {
        public GameObject particles;
        public SimpleAudioEvent _audio;
        private AudioSourcePoolManager _audioPool;
        private float _hitForce = 20f;
        private ShipType _owner;

        [SerializeField] private AudioClip _fallSound1;
        [SerializeField] private AudioClip _fallSound2;
        private void Start()
        {
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();
            var audioSource = GetComponent<AudioSource>();
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                audioSource.clip = _fallSound1;
            }
            else
            {
                audioSource.clip = _fallSound2;
            }

            audioSource.Play();
        }

        public void Init(ShipType type)
        {
            _owner = type;
        }

        private void OnCollisionEnter(Collision other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(transform.position, _hitForce);

            DamageShip(other.gameObject);
           
            Destroy(gameObject);
        }

        void DamageShip(GameObject other)
        {
            var ship = other.gameObject.GetComponentInParent<ShipCondition>();

            if (ship != null)
            {
                if (ship.ShipType != _owner && ship.ShipType == ShipType.Enemy)
                {
                    ship.TakeDamage();
                }
                
                var particleSystem = Instantiate(particles, transform.position, Quaternion.identity);
                if (particleSystem == null)
                {
                    Debug.LogError("Missing particle system prefab");
                }
                else
                {
                    var systems = particleSystem.GetComponentsInChildren<ParticleSystem>();
                    foreach (var s in systems)
                    {
                        s.Play();
                    }
                }
                _audioPool.PlayAudioEvent(_audio, transform.position);
            }
        }
    }
}