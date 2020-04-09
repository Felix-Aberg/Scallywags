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
        public bool singleUse;
        private Player _pickedUpBy;
        private Rigidbody _rb;
        public ItemType itemType;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            if (itemType == ItemType.NotSet)
            {
                Debug.LogError("You must set item type for item");
            }
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
            
            var t = transform;
            t.SetParent(player.gameObject.transform);
            t.localRotation = Quaternion.identity;

            return this;
        }

        public void Drop()
        {
            if (_pickedUpBy == null) return;
            
            _rb.detectCollisions = true;
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.None;
            
            _pickedUpBy = null;
            transform.SetParent(null);
        }
    }
}