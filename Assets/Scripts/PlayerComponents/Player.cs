using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ScallyWags
{
    /// <summary>
    /// This script constructs the player
    /// </summary>
    public class Player : MonoBehaviour, IEntity
    {
        public int Index => _index;
        [SerializeField] private int _index;
        private InputHandler _inputHandler;
        private PlayerController _playerController;
        [SerializeField] private Pickup _pickup;
        [SerializeField] private Interact _interact;

        private AnimationController _animationController;

        // Monobehaviors
        private Rigidbody _rigidbody;

        public void Init(int index)
        {
            _index = index;
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.mass = 10;
            
            _pickup = new Pickup();
            _interact = new Interact();
            _playerController = new PlayerController();
            _inputHandler = new InputHandler();
            _animationController = new AnimationController(GetComponent<Animator>(), _rigidbody, _pickup);
            
            _pickup.Init(transform, _animationController, GetComponentInChildren<RightArmTarget>());
            _interact.Init(_animationController);

        }

        public void Tick()
        {
            // Get inputs
            Inputs inputs = new Inputs();
            inputs = _inputHandler.GetInputs(_index);
            
            // Handle input
            _playerController.Tick(transform, inputs.horizontal, inputs.vertical);
            _pickup.Tick(this, inputs.pickUpPressed, inputs.pickUpDown, inputs.pickUpReleased);
            _interact.Tick(_pickup.PickedUpItem, this, inputs.interActPressed);
            
            _animationController.Tick();
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
    }
}