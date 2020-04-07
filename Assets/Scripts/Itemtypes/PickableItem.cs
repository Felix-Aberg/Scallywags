using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// Adding this to game object makes the item pickable buy player
    /// </summary>
    public class PickableItem : MonoBehaviour, IPickable
    {
        private Player _pickedUpBy;
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public bool IsAvailable()
        {
            return _pickedUpBy == null;
        }

        public GameObject GetObject()
        {
            return gameObject;
        }

        public IPickable Pickup(Player player)
        {
            if (_pickedUpBy != null) return null;

            _rb.detectCollisions = false;
            _pickedUpBy = player;
            gameObject.transform.SetParent(player.gameObject.transform);
            return this;
        }

        public void Drop()
        {
            if (_pickedUpBy == null) return;
            
            _rb.detectCollisions = true;
            _pickedUpBy = null;
            gameObject.transform.SetParent(null);
        }
    }
}