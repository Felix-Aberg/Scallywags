using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        private AudioSourcePoolManager _audioSourcePoolManager;
        
        // Monobehaviors
        private Rigidbody _rigidbody;
        private BoxCollider _triggerCollider;
        private Respawnable _respawnable;
        private Drown _drown;
        private Emote _emote;

        // AudioEvents
        [SerializeField] SimpleAudioEvent _emoteAudio;

        public void Init(int index)
        {
            _index = index;
            var model = transform.GetChild(index);
            model.gameObject.SetActive(true);

            _audioSourcePoolManager = FindObjectOfType<AudioSourcePoolManager>();
            
            _pickup = new Pickup();
            _interact = new Interact();
            _jump = new Jump();
            _emote = new Emote();
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
            _ragdoll = new Ragdoll(rigidBodyColliders, _ragdollRigidBodies, rigidbodyBoxcolliders, capsuleCollider, GetComponentInChildren<Animator>());
            _ragdoll.DisableRagdoll(_ragdollRigidBodies);

            // Enable trigger collider
            _triggerCollider = GetComponent<BoxCollider>();
            _triggerCollider.enabled = true;
            
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.mass = 1;
            _rigidbody.drag = 0f;

            _animationController = gameObject.AddComponent<AnimationController>();
            _animationController.Init(GetComponentInChildren<Animator>(), _rigidbody, _pickup, _jump);

            _pickup.Init(transform, _animationController, GetComponentInChildren<RightArmTarget>());
            _interact.Init(_animationController);
            _jump.Init(transform);
            _emote.Init(_emoteAudio, _audioSourcePoolManager, _animationController);

            _respawnable = GetComponent<Respawnable>();
            _drown = GetComponent<Drown>();
        }

        public void Tick()
        {
            if (_isDead)
            {
                _respawnable.Tick();
                return;
            }

            _drown.Tick();

            // Get inputs
            Inputs inputs = new Inputs();
            inputs = _inputHandler.GetInputs(_index);

            // Handle input
            _playerController.Tick(transform, inputs.horizontal, inputs.vertical);
            _pickup.Tick(this, inputs.pickUpPressed, inputs.pickUpDown, inputs.pickUpReleased);
            _interact.Tick(_pickup.PickedUpItem, this, inputs.interActPressed);
            _jump.Tick(transform, _rigidbody, inputs.jumpPressed, inputs.jumpDown);
            _emote.Tick(inputs.emoteDown);

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
            _triggerCollider.enabled = true;
            Drop();
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