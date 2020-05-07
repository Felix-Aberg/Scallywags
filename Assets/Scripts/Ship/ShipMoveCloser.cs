using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ScallyWags
{
    public class ShipMoveCloser : MonoBehaviour
    {
        private float _moveDistance = 15;
        private Vector3 _moveTarget;
        private ShipCondition _ship;
        [SerializeField] private float _speed = 0.03f;

        private void Start()
        {
            _ship = GetComponent<ShipCondition>();
            _moveTarget = new Vector3(0, 0, _moveDistance);
        }

        void Update()
        {
            if (_ship.IsSinking()) return;
            
            _moveTarget = new Vector3(0, 0, _moveDistance);
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(0, transform.position.y, _moveTarget.z), _speed);
        }
    }
}