using System.Collections.Generic;
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
        private float _yOffset = 0.7f;
        private float _xOffset = 1.2f;
        
        [SerializeField] float _throwStrength = 0f;
        private float _initialThrowStrength = -5f; // Inverse time until it throws instead of dropping
        private float _maxThrowStrength = 20f; //Max throw strength
        private float _strenghtFactor = 35f; // Throw charge speed (per second)

        private Transform _transform;

        public void Init(Transform transform)
        {
            _transform = transform;
        }

        public void Tick(Player player, bool pickUpPressed, bool pickupDown, bool pickUpReleased)
        {
            if (_pickedUpItem != null)
            {
                _pickedUpItem.transform.position = CalculateTargetPos(_transform);
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
                        
            if (pickUpReleased)
            {
                var itemToPickUp = GetClosestItem(player);
                if (itemToPickUp != null)
                {
                    if (TryToPickUp(itemToPickUp))
                    {
                        PickUp(itemToPickUp as PickableItem, player);
                    }
                }
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
                if (_itemsNear.Contains(gameObject)) return;
                _itemsNear.Add(gameObject);
            }
        }
        
        public void RemoveItem(GameObject gameObject)
        {
            var item = gameObject.GetComponent<PickableItem>();
            if (item != null)
            {
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

        private Vector3 CalculateTargetPos(Transform transform)
        {
            var targetPos = transform.position;
            targetPos.y += _yOffset;
            targetPos += transform.forward * _xOffset;
            return targetPos;
        }

        private void PickUp(PickableItem item, Player player)
        {
            if (_pickedUpItem == null)
            {
                _pickedUpItem = item.Pickup(player) as PickableItem;
                _pickedUpItem.transform.DOMove(CalculateTargetPos(_transform), 0.2f);
            }
        }

        public void Throw()
        {
            if (_pickedUpItem == null) return;
            
            _pickedUpItem.Drop();
            _pickedUpItem.GetComponent<Rigidbody>().AddForce(_pickedUpItem.transform.forward * _throwStrength, ForceMode.Impulse);
            _pickedUpItem = null;
            _itemsNear.Clear();
        }
    }
}