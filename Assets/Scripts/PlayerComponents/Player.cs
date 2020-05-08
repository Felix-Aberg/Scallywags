using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// This script constructs the player
    /// </summary>
    public class Player : MonoBehaviour, IEntity, IDamageable
    {
        public int Index => _index;
        [SerializeField] private int _index;
        
        private bool _isDead;
        
        private InputHandler _inputHandler;
        private PlayerController _playerController;
        [SerializeField] private Pickup _pickup;
        [SerializeField] private Interact _interact;
        [SerializeField] private Jump _jump;

        private AnimationController _animationController;
        private Ragdoll _ragdoll;
        private Vector3 _hitPos;
        private Rigidbody[] _ragdollRigidBodies;

        // Monobehaviors
        private Rigidbody _rigidbody;

        public void Init(int index)
        {
            _index = index;


            _pickup = new Pickup();
            _interact = new Interact();
            _jump = new Jump();
            _playerController = new PlayerController();
            _inputHandler = new InputHandler();

            // Find rigidbody colliders
            var rigidBodyColliders = GetComponentsInChildren<CapsuleCollider>();
            var rigidbodyBoxcolliders = GetComponentsInChildren<BoxCollider>();
            
            // Add player collider
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.height = 1.98f;
            capsuleCollider.radius = 0.5f;
            capsuleCollider.center = new Vector3(0,1,0);
            
            _ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
            _ragdoll = new Ragdoll(rigidBodyColliders, _ragdollRigidBodies, rigidbodyBoxcolliders, capsuleCollider, GetComponent<Animator>());
            _ragdoll.DisableRagdoll(_ragdollRigidBodies);

            // Enable trigger collider
            GetComponent<BoxCollider>().enabled = true;
            
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.mass = 50;

            _animationController = new AnimationController(GetComponent<Animator>(), _rigidbody, _pickup, _jump);

            _pickup.Init(transform, _animationController, GetComponentInChildren<RightArmTarget>());
            _interact.Init(_animationController);
            _jump.Init(transform);
        }

        public void Tick()
        {
            if (_isDead) return;
            
            // Get inputs
            Inputs inputs = new Inputs();
            inputs = _inputHandler.GetInputs(_index);

            // Handle input
            _playerController.Tick(transform, inputs.horizontal, inputs.vertical);
            _pickup.Tick(this, inputs.pickUpPressed, inputs.pickUpDown, inputs.pickUpReleased);
            _interact.Tick(_pickup.PickedUpItem, this, inputs.interActPressed);
            _jump.Tick(transform, _rigidbody, inputs.jumpPressed);

            _animationController.Tick();
        }

        public GameObject GetObject()
        {
            return gameObject;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void Drop()
        {
            _pickup.Throw();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Detect nearby items and set them as target to pickup and interact scripts
            var item = other.gameObject;
            _pickup.SetItem(item);
            _interact.SetItem(item);
        }

        private void OnTriggerExit(Collider other)
        {
            var item = other.gameObject;
            _pickup.RemoveItem(item);
            _interact.RemoveItem(item);
        }

        public void TakeDamage()
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(Vector3 pos, float hitForce)
        {
            _isDead = true;
            var dir = transform.position - pos;
            Die(dir.normalized, hitForce);
        }

        public Vector3 GetPos()
        {
            return transform.position;
        }

        public void Respawn()
        {
            _isDead = false;
            _ragdoll.DisableRagdoll(_ragdollRigidBodies);
        }

        /// <summary>
        /// Takes in direction player was hit and hit force
        /// </summary>
        /// <param name="hitDir"></param>
        /// <param name="hitForce"></param>
        private void Die(Vector3 hitDir, float hitForce = 1f)
        {
            Drop();
            _ragdoll.EnableRagdoll(hitDir, hitForce);
        }
    }
}