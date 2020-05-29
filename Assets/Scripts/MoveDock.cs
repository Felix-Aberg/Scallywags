using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ScallyWags
{

    public class MoveDock : MonoBehaviour
    {
        private Vector3 _targetPos;
        private List<GameObject> _children = new List<GameObject>();
        
        public void Start()
        {
            _targetPos = FindObjectOfType<DockTargetPos>().transform.position;
            foreach (var t in GetComponentsInChildren<Transform>())
            {
                if (t.gameObject == this.gameObject) continue;
                t.gameObject.SetActive(false);
                _children.Add(t.gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.StartListening("RoundOver", MoveDockCloser);
        }

        private void OnDisable()
        {
            EventManager.StopListening("RoundOver", MoveDockCloser);
        }
        
        private void MoveDockCloser(EventManager.EventMessage msg)
        {
            foreach (var child in _children)
            {
                child.SetActive(true);
            }
 
            transform.DOMove(_targetPos, 10f);
        }
    }
}
