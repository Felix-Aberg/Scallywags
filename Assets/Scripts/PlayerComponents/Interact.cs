using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace ScallyWags
{
    [System.Serializable]
    public class Interact
    {
        private AnimationController _animationController;
        [SerializeField] private List<GameObject> _itemsNear = new List<GameObject>();
        private bool _interacting;

        public void Init(AnimationController animationController)
        {
            _animationController = animationController;
        }
        
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
            var item = gameObject.GetComponent<IInteractable>() as InteractableItem;
            if (item != null)
            {
                item.DisableUI();
                if (_itemsNear.Contains(gameObject))
                {
                    _itemsNear.Remove(gameObject);
                }
            }
        }

        public void Tick(PickableItem currentItem, Player player, bool interactPressed)
        {           
            if (currentItem == null)
            {
                _interacting = false;
                return;
            }
            
            _animationController.Interact(currentItem.itemType, _interacting);
            
            if (interactPressed)
            {
                _animationController.InteractTrigger(currentItem.itemType);
            }
            

            var closestItem = GetClosestItem(player, currentItem) as InteractableItem;
            if (closestItem == null)
            {
                _interacting = false;
                return;
            }
                
            if (interactPressed)
            {
                _interacting = true;
                RefreshItems();
                closestItem.GetObject().GetComponent<ItemHighlight>()?.HighlightItem(null);
                closestItem.DisableUI();
            }

            if (_interacting)
            {
                closestItem.Interact(currentItem, player);
            }
        }

        private void RefreshItems()
        {
            List<GameObject> newList = new List<GameObject>();
            foreach (var item in _itemsNear)
            {
                if (item.gameObject.activeInHierarchy == false) continue;
                
                var entity = item.gameObject.GetComponent<InteractableItem>();

                if (!entity.enabled)
                {
                    continue;
                }
                
                newList.Add(item);
            }

            _itemsNear = newList;
        }

        private IInteractable GetClosestItem(Player player, PickableItem currentItem)
        {
            GameObject closest = null;
            var closestDist = float.MaxValue;
            foreach (var item in _itemsNear)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    continue;
                }
                
                if (item == null)
                {
                    continue;
                }
               
                var interactable = item.GetComponent<InteractableItem>();
                
                if (interactable.CanBeUsed(currentItem) == false)
                {
                    continue;
                }

                if (closest == null)
                {
                    closest = item;
                }

                // UI
                interactable.EnableUI();
                item.GetComponent<ItemHighlight>()?.HighlightItem(_itemsNear);

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