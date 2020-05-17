using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ScallyWags
{

    public class MoveDock : MonoBehaviour
    {
        private Vector3 _targetPos;
        
        public void Start()
        {
            _targetPos = FindObjectOfType<DockTargetPos>().transform.position;
            EventManager.StartListening("RoundOver", MoveDockCloser);
        }

        private void OnDisable()
        {
            EventManager.StopListening("RoundOver", MoveDockCloser);
        }
        
        private void MoveDockCloser(EventManager.EventMessage msg)
        {
            transform.DOMove(_targetPos, 14f);
        }
    }
}
