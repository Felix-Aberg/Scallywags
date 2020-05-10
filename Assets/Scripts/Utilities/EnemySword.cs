using System;
using UnityEngine;

namespace ScallyWags
{
    public class EnemySword : MonoBehaviour
    {
        private BoxCollider _attackCollider;
        private float _hitForce = 10f;
        private float _attackTimer;
        private float _attackDelay = 1f;

        private void Start()
        {
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
            _attackTimer += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision other)
        {
            var target = other.gameObject.GetComponent<Player>();
            if (target != null)
            {
                if (_attackTimer > _attackDelay)
                {
                    target.TakeDamage(transform.position, _hitForce);
                    _attackTimer = 0;
                }
            }
        }

        public void EnableCollider(bool enabled)
        {
          _attackCollider.enabled = enabled;
        }
    }
}