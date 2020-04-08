using System.Collections.Generic;
using UnityEngine;

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
        private float _Y_Offset = 0.5f;


        public void Tick(Transform transform, Player player, bool pickUpPressed)
        {
            if (_pickedUpItem != null)
            {
                _pickedUpItem.GetObject().transform.position = CalculateTargetPos(transform);
            }
            Inputs(player, pickUpPressed);
        }

        private void Inputs(Player player, bool pickUpPressed)
        {
            if (pickUpPressed)
            {
                if (_pickedUpItem != null)
                {
                    if (TryToDrop())
                    {
                        Drop();
                    }
                }
                else
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
            targetPos.y += _Y_Offset;
            targetPos += transform.forward;
            return targetPos;
        }

        private void PickUp(PickableItem item, Player player)
        {
            if (_pickedUpItem == null)
            {
                _pickedUpItem = item.Pickup(player) as PickableItem;
            }
        }

        private bool TryToDrop()
        {
            return _pickedUpItem != null;
        }

        public void Drop()
        {
            if (_pickedUpItem == null) return;
            
            _pickedUpItem.Drop();
            _pickedUpItem = null;
            _itemsNear.Clear();
        }
    }
}