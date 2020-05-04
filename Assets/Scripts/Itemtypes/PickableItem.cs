using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace ScallyWags
{
    /// <summary>
    /// Adding this to game object makes the item pickable by player
    /// </summary>
    public class PickableItem : MonoBehaviour, IPickable
    {
        public ItemType itemType;
        public bool singleUse;

        public IEntity PickedUpBy => _pickedUpBy;
        private IEntity _pickedUpBy;
        private Rigidbody _rb;
        
        private SphereCollider _sphereCollider;
        private BoxCollider _boxCollider;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            if (itemType == ItemType.NotSet)
            {
                Debug.LogError("You must set item type for item");
            }

            _boxCollider = GetComponent<BoxCollider>();
            _sphereCollider = GetComponent<SphereCollider>();
        }

        public bool IsAvailable()
        {
            return _pickedUpBy == null;
        }

        public GameObject GetObject()
        {
            return gameObject;
        }

        public IPickable Pickup(IEntity entity)
        {
            if (_pickedUpBy != null) return null;

            if (_boxCollider) _boxCollider.isTrigger = true;
            if (_sphereCollider) _sphereCollider.isTrigger = true;
           // _rb.detectCollisions = false;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            
            _pickedUpBy = entity;
            
            var t = transform;
            t.localRotation = Quaternion.identity;
            t.SetParent(entity.GetObject().GetComponentInChildren<RightArmTarget>().transform);
            t.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.Euler(0,-90,0);

            return this;
        }

        public void Drop()
        {
            if (_pickedUpBy == null) return;
            
            if (_boxCollider) _boxCollider.isTrigger = false;
            if (_sphereCollider) _sphereCollider.isTrigger = false;
           //  _rb.detectCollisions = true;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.None;

            Rigidbody _parentRb = _pickedUpBy.GetObject().GetComponent<Rigidbody>();
            if (_parentRb == null) Debug.Log("Parent is null!");

            _pickedUpBy = null;
            transform.SetParent(null);
            _rb.AddForce(_parentRb.velocity, ForceMode.VelocityChange);
        }
    }
}