using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace ScallyWags
{
    [System.Serializable]
    public class Interact
    {
        [SerializeField] private List<GameObject> _itemsNear = new List<GameObject>();

        public void SetItem(GameObject gameObject)
        {
            var item = gameObject.GetComponent<IInteractable>();
            if (item != null)
            {
                if (_itemsNear.Contains(gameObject)) return;
                _itemsNear.Add(gameObject);
            }
        }
        
        public void RemoveItem(GameObject gameObject)
        {
            var item = gameObject.GetComponent<IInteractable>();
            if (item != null)
            {
                if (_itemsNear.Contains(gameObject))
                {
                    _itemsNear.Remove(gameObject);
                }
            }
        }

        public void Tick(PickableItem item, Player player, bool interactPressed)
        {
            if (item == null) return;
            
            if (interactPressed)
            {
                var closestItem = GetClosestItem(player);
                closestItem?.Interact(item, player);
                RefreshItems();
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

        private IInteractable GetClosestItem(Player player)
        {
            GameObject closest = null;
            var closestDist = float.MaxValue;
            foreach (var item in _itemsNear)
            {
                if (item == null) continue;
                
                var currentDist = Vector3.Distance(player.transform.position, item.transform.position);
                if (currentDist < closestDist)
                {
                    closest = item;
                    closestDist = currentDist;
                }
            }

            return closest?.GetComponent<InteractableItem>();
        }
    }
}