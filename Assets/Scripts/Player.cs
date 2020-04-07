using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// This script constructs the player
    /// </summary>
    public class Player : MonoBehaviour, IEntity
    {
        public int Index => _index;
        [SerializeField] private int _index;
        private PlayerController _playerController;
        [SerializeField] private Pickup _pickup;
        [SerializeField] private Interact _interact;

        // Monobehaviors
        private Rigidbody _rigidbody;

        public void Init(int index)
        {
            _index = index;
            gameObject.AddComponent<CapsuleCollider>();
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            
            _pickup = new Pickup();
            _interact = new Interact();
            _playerController = new PlayerController();
        }

        public void Tick()
        {
            // Get inputs from controllers here for example (index is controller number):
            // var vertical = _inputManager.GetVertical(index);
            
            // TODO move this to input manager
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool pickUpPressed = Input.GetButtonDown("Fire1");
            bool interActPressed = Input.GetButtonDown("Fire2");
            
            // Handle input
            _playerController.Tick(transform, horizontal, vertical);
            _pickup.Tick(gameObject.transform, this, pickUpPressed);
            _interact.Tick(_pickup.PickedUpItem, this, interActPressed);
        }
        
        public void Drop()
        {
            _pickup.Drop();
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