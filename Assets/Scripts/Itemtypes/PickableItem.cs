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
        private int _layer;
        
                
        // UI
        private CreateUIElement _createUIElement;
        private ButtonPrompt _interactUI;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            if (itemType == ItemType.NotSet)
            {
                Debug.LogError("You must set item type for item");
            }

            _boxCollider = GetComponent<BoxCollider>();
            _sphereCollider = GetComponent<SphereCollider>();
            _layer = gameObject.layer;
            
            _createUIElement = FindObjectOfType<CreateUIElement>();
            //CreateInteractUI();
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
            
            gameObject.layer = 16; // Do not collide with player
            
           // if (_boxCollider) _boxCollider.isTrigger = true;
           // if (_sphereCollider) _sphereCollider.isTrigger = true;
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
            
            gameObject.layer = _layer;
            
          //  if (_boxCollider) _boxCollider.isTrigger = false;
          //  if (_sphereCollider) _sphereCollider.isTrigger = false;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.None;

            Rigidbody _parentRb = _pickedUpBy.GetObject().GetComponent<Rigidbody>();
            if (_parentRb == null) Debug.Log("Parent is null!");

            _pickedUpBy = null;
            transform.SetParent(null);
            _rb.AddForce(_parentRb.velocity, ForceMode.VelocityChange);
        }
        
        private void CreateInteractUI()
        {
            var interactUI = _createUIElement.CreateElement(UIElement.Interact, transform.position);
            _interactUI = interactUI.GetComponent<ButtonPrompt>();
            _interactUI.Init(transform.position, 2f);
            DisableUI();
        }

        public void DisableUI()
        {
           // _interactUI.gameObject.SetActive(false);
        }
        
        public void EnableUI()
        {
           // _interactUI.gameObject.SetActive(true);
        }
    }
}