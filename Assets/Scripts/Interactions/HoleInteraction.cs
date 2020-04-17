using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScallyWags
{
    public class HoleInteraction : MonoBehaviour, IInteraction
    {
        private ShipCondition _shipCondition;
        private ParticleSystem[] _particleSystem;
        [SerializeField] private int hammerHits = 0;
        private int hitsRequired = 5;

        private void Start()
        {
            _particleSystem = GetComponentsInChildren<ParticleSystem>();
            _shipCondition = GetComponentInParent<ShipCondition>();
        }

        public void Act()
        {
            // TODO hammer sounds
            hammerHits++;
            foreach (var p in _particleSystem)
            {
                p.Play();
            }

            if (hammerHits >= hitsRequired)
            {
                Fix();
            }
        }

        private void Fix()
        {
            Debug.Log("Hole fixed");
            hammerHits = 0;
            _shipCondition.FixDamage();
            gameObject.SetActive(false);
        }
    }
}
