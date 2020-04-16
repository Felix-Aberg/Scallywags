using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScallyWags
{
    [RequireComponent(typeof(ParticleSystem))]
    public class HoleInteraction : MonoBehaviour, IInteraction
    {
        private ParticleSystem _particleSystem;
        private int hammerHits = 0;
        private int hitsRequired = 5;

        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Act()
        {
            // TODO hammer sounds effects etc
            hammerHits++;
            _particleSystem.Play();
            
            if (hammerHits >= hitsRequired)
            {
                Fix();
            }
        }

        private void Fix()
        {
            gameObject.SetActive(false);
            hammerHits = 0;
        }

        public void CreateHole()
        {
            gameObject.SetActive(true);
        }
    }
}
