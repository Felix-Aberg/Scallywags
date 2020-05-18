using System;
using UnityEngine;

namespace ScallyWags
{
    public class EnemySword : MonoBehaviour
    {
        private BoxCollider _attackCollider;
        private float _hitForce = 10f;
        private float _attackTimer;
        private float _attackDelay = 2f;
        private Skeleton _skeleton;

        private void Start()
        {
            _skeleton = GetComponentInParent<Skeleton>();
            _attackCollider = GetComponent<BoxCollider>();
            _attackCollider.isTrigger = false;
            _attackCollider.enabled = true;
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void Update()
        {
            if (_skeleton.IsDead())
            {
                _attackCollider.enabled = false;
            }
        }

        public void EnableCollider(bool enabled)
        {
            if (_skeleton.IsDead())
            {
                return;
            }
            _attackCollider.enabled = enabled;
        }
    }
}