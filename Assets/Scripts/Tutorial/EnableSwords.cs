using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class EnableSwords : MonoBehaviour
    {
        private Transform[] _swords;
        void Start()
        {
            _swords = GetComponentsInChildren<Transform>();
            
            foreach (var c in _swords)
            {
                if (c == transform) continue;
                c.gameObject.SetActive(false);
            }
        }
    
        private void OnEnable()
        {
            EventManager.StartListening("SpawnEntity", EnableGameObjects);
        }
    
        private void OnDisable()
        {
            EventManager.StopListening("SpawnEntity", EnableGameObjects);
        }
    
        private void EnableGameObjects(EventManager.EventMessage args)
        {
            foreach (var c in _swords)
            {
                c.gameObject.SetActive(true);
            }
        }
    }
}
