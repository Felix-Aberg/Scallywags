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

            var rigidBodyColliders = GetComponentsInChildren<CapsuleCollider>();
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.height = 1.98f;
            capsuleCollider.radius = 0.5f;
            capsuleCollider.center = new Vector3(0,1,0);
            // DODO
            
            _ragdoll = new Ragdoll(rigidBodyColliders, GetComponentsInChildren<Rigidbody>(), capsuleCollider, GetComponent<Animator>());
            _ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
            _ragdoll.DisableRagdoll(_ragdollRigidBodies);
            
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
            _isDead = true;
            Die();
        }

        public void Respawn()
        {
            _isDead = false;
            _ragdoll.DisableRagdoll(_ragdollRigidBodies);
        }

        private void Die()
        {
            Drop();
            _ragdoll.EnableRagdoll((transform.position - _hitPos).normalized);
        }

        public void Push(Vector3 pos)
        {
            _hitPos = pos;
        }
    }
}