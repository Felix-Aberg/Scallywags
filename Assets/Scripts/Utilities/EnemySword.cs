using System;
using UnityEngine;

namespace ScallyWags
{
    public class EnemySword : MonoBehaviour
    {
        private BoxCollider _attackCollider;
        private float _hitForce = 10f;

        private void Start()
        {
            _attackCollider = GetComponent<BoxCollider>();
            _attackCollider.isTrigger = false;
            _attackCollider.enabled = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            var target = other.gameObject.GetComponent<Player>();
            if (target != null) {
                target.TakeDamage(transform.position, _hitForce);
                _attackCollider.enabled = false;
            }
        }

        public void EnableCollider(bool enabled)
        {
            _attackCollider.enabled = enabled;
        }
    }
}