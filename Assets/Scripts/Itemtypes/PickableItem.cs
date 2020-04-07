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
        public bool singleUse;
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
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            
            _pickedUpBy = player;
            gameObject.transform.SetParent(player.gameObject.transform);
            transform.rotation = Quaternion.identity;
            
            return this;
        }

        public void Drop()
        {
            if (_pickedUpBy == null) return;
            
            _rb.detectCollisions = true;
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.None;
            
            _pickedUpBy = null;
            gameObject.transform.SetParent(null);
        }
    }
}