using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ScallyWags
{
    public class ShipMoveCloser : MonoBehaviour
    {
        private Vector3 _moveTarget = new Vector3(0, 0, 20);
        private ShipCondition _ship;

        private void Start()
        {
            _ship = GetComponent<ShipCondition>();
        }

        void Update()
        {
            if (_ship.IsSinking()) return;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(0, transform.position.y, _moveTarget.z), 0.05f);
        }
    }
}