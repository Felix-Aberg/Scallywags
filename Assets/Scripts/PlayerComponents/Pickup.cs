using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using DG.Tweening;

namespace ScallyWags
{
    /// <summary>
    /// Pickup/drop logic for IPickable items
    /// </summary>
    [System.Serializable]
    public class Pickup
    {
        public PickableItem PickedUpItem => _pickedUpItem;
        private PickableItem _pickedUpItem;
        [SerializeField] private List<GameObject> _itemsNear = new List<GameObject>();
        
        [SerializeField] float _throwStrength = 0f;
        private float _initialThrowStrength = -5f; // Inverse time until it throws instead of dropping
        private float _maxThrowStrength = 20f; //Max throw strength
        private float _strenghtFactor = 35f; // Throw charge speed (per second)

        private Transform _transform;
        private AnimationController _animationController;
        private RightArmTarget _rightArmTarget;

        public void Init(Transform transform, AnimationController animationController, RightArmTarget rightArmTarget)
        {
            _transform = transform;
            _animationController = animationController;
            _rightArmTarget = rightArmTarget;
        }

        public void Tick(Player player, bool pickUpPressed, bool pickupDown, bool pickUpReleased)
        {
            if (_pickedUpItem != null)
            {
                _pickedUpItem.transform.position = _rightArmTarget.transform.position;
            }
            Inputs(player, pickUpPressed, pickupDown, pickUpReleased);
        }

        private void Inputs(Player player, bool pickUpPressed, bool pickupDown, bool pickUpReleased)
        {
            if (_pickedUpItem != null)
            {
                HandleThrowing(player, pickUpPressed, pickupDown, pickUpReleased);
            }
            else
            {
                HandlePickingUp(player, pickUpPressed, pickupDown, pickUpReleased);
            }
        }

        private void HandlePickingUp(Player player, bool pickUpPressed, bool pickupDown, bool pickUpReleased)
        {
            var itemToPickUp = GetClosestItem(player);

            if (itemToPickUp == null) return;

            if (!TryToPickUp(itemToPickUp)) return;

            var pickable = itemToPickUp as PickableItem;
            pickable.EnableUI();
            pickable.GetComponent<ItemHighlight>()?.HighlightItem(_itemsNear);

            if (pickUpReleased)
            {
                PickUp(itemToPickUp as PickableItem, player);
                RefreshItems();
            }
        }

        private void HandleThrowing(Player player, bool pickUpPressed, bool pickupDown, bool pickUpReleased)
        {
            if (pickupDown)
            {
                _throwStrength = _initialThrowStrength;
            }
            
            if (pickUpPressed)
            {
                _throwStrength += Time.deltaTime * _strenghtFactor;
            }
  
            if (pickUpReleased)
            {
                _throwStrength = Mathf.Clamp(_throwStrength, 0f, _maxThrowStrength);
                Throw();
                _animationController.Throw();
            }
        }

        private void RefreshItems()
        {
            List<GameObject> newList = new List<GameObject>();
            foreach (var item in _itemsNear)
            {
                if (item == null) continue;
                
                newList.Add(item);
            }
            _itemsNear = newList;
        }

        private IPickable GetClosestItem(Player player)
        {
            GameObject closest = null;
            var closestDist = float.MaxValue;
            foreach (var item in _itemsNear)
            {
                var currentDist = Vector3.Distance(player.transform.position, item.transform.position);
                if (currentDist < closestDist)
                {
                    closest = item;
                    closestDist = currentDist;
                }
            }

            return closest?.GetComponent<PickableItem>();
        }

        public void SetItem(GameObject gameObject)
        {
            var item = gameObject.GetComponent<IPickable>();
            if (item != null)
            {
                if (item == _pickedUpItem) return;
                if (_itemsNear.Contains(gameObject)) return;
                _itemsNear.Add(gameObject);
            }
        }
        
        public void RemoveItem(GameObject gameObject)
        {
            var item = gameObject.GetComponent<PickableItem>();
            if (item != null)
            {
                item.DisableUI();
                if (_itemsNear.Contains(gameObject))
                {
                    _itemsNear.Remove(gameObject);
                }
            }
        }

        private bool TryToPickUp(IPickable itemToPickUp)
        {
            return _pickedUpItem == null && itemToPickUp.IsAvailable();
        }

        private void PickUp(PickableItem item, Player player)
        {
            if (_pickedUpItem == null)
            {
                _pickedUpItem = item.Pickup(player) as PickableItem;
                _itemsNear.Remove(item.GetObject());
            }
        }

        public void Throw()
        {
            if (_pickedUpItem == null) return;
            
            _pickedUpItem.Drop();
            _pickedUpItem.GetComponent<Rigidbody>().AddForce(_pickedUpItem.transform.right * _throwStrength, ForceMode.Impulse);
            _pickedUpItem = null;
            _itemsNear.Clear();
        }
    }
}