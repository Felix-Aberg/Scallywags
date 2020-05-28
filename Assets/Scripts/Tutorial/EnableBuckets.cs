using System;
using UnityEngine;

namespace ScallyWags
{
    public class EnableBuckets : MonoBehaviour
    {
        private Transform[] _buckets;
        void Start()
        {
            _buckets = GetComponentsInChildren<Transform>();
        
            foreach (var c in _buckets)
            {
                if (c == transform) continue;
                c.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            EventManager.StartListening("Intro2", EnableGameObjects);
        }

        private void OnDisable()
        {
            EventManager.StopListening("Intro2", EnableGameObjects);
        }

        private void EnableGameObjects(EventManager.EventMessage args)
        {
            foreach (var c in _buckets)
            {
                c.gameObject.SetActive(true);
            }
        }
        
        public void EnableTools()
        {
            foreach (var c in _buckets)
            {
                c.gameObject.SetActive(true);
            }
        }
    }
}