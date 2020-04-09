﻿using System;
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
        private InputHandler _inputHandler;
        private PlayerController _playerController;
        [SerializeField] private Pickup _pickup;
        [SerializeField] private Interact _interact;
        [SerializeField] private bool useKeyboard;

        private float _deadDepth = -10f;

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
            _inputHandler = gameObject.AddComponent<InputHandler>();
        }

        public void Tick()
        {
            // Get inputs from controllers here for example (index is controller number):
            // var vertical = _inputManager.GetVertical(index);
            
            float horizontal;
            float vertical;
            bool pickUpPressed;
            bool interActPressed;
            bool pickUpReleased;
            bool pickUpDown;

            if (useKeyboard) // Just for debugging
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                pickUpPressed = Input.GetButton("Fire1");
                pickUpReleased = Input.GetButtonUp("Fire1");
                pickUpDown = Input.GetButtonDown("Fire1");
                interActPressed = Input.GetButtonDown("Fire2");
            }
            else
            {
                horizontal = _inputHandler.GetXAxis(_index);
                vertical = _inputHandler.GetYAxis(_index);
                pickUpPressed = _inputHandler.GetPickupDown(_index);
                pickUpDown = _inputHandler.GetPickup(_index);
                pickUpReleased = _inputHandler.GetPickupUp(_index);
                interActPressed = _inputHandler.GetInteractDown(_index);
            }
            
            // Handle input
            _playerController.Tick(transform, horizontal, vertical);
            _pickup.Tick(gameObject.transform, this, pickUpPressed, pickUpDown, pickUpReleased);
            _interact.Tick(_pickup.PickedUpItem, this, interActPressed);
        }

        public bool IsDead()
        {
            return transform.position.y < _deadDepth;
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