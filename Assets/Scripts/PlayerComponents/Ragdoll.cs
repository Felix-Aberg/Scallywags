using Cinemachine;
using UnityEngine;

namespace ScallyWags
{
    public class Ragdoll
    {
        private Rigidbody[] _rigidBodies;
        private CapsuleCollider _colliderCollider;
        private CapsuleCollider[] _rigidBodyColliders;
        private Animator _animator;
        private float impactStrength = 50f;

        public Ragdoll(CapsuleCollider[] rigidbodyColliders, Rigidbody[] rigidBodies, CapsuleCollider colliderCollider, Animator animator)
        {
            _rigidBodyColliders = rigidbodyColliders;
            _rigidBodies = rigidBodies;
            _colliderCollider = colliderCollider;
            _animator = animator;
        }

        public void EnableRagdoll(Vector3 dir)
        {
            _colliderCollider.enabled = false;
            
            foreach (var rb in _rigidBodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.detectCollisions = true;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }

            foreach (var collider in _rigidBodyColliders)
            {
                collider.enabled = true;
            }
            
            _animator.enabled = false;

            // Add push force
            foreach (var rb in _rigidBodies)
            {
                rb.AddForce(dir * impactStrength, ForceMode.Impulse);
            }
        }

        public void DisableRagdoll(Rigidbody[] rbs)
        {
            _animator.enabled = true;
            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }
            
            foreach (var collider in _rigidBodyColliders)
            {
                collider.enabled = false;
            }

            _colliderCollider.enabled = true;
        }
    }
}