using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class InteractableItem : MonoBehaviour, IInteractable
    {
        // Specifies which type of item can be used on this item (Usable items in data folder)
        [SerializeField] private UsableItems _usableItems;
        
        // UI
        private CreateUIElement _createUIElement;
        private ButtonPrompt _interactUI;

        void Start()
        {
            _createUIElement = FindObjectOfType<CreateUIElement>();
            CreateInteractUI();
        }

        public void Interact(PickableItem item, Player player)
        {
            foreach (var usable in _usableItems.itemList)
            {
                if (item.itemType != usable.itemType) continue;
                
                Use(item);

                if (!item.singleUse) break;
                
                player.Drop();
                var respawnable = item.GetComponent<Respawnable>();
                if (respawnable)
                {
                    respawnable.Respawn();
                }
                else
                {
                    Destroy(item);
                }
            }
        }

        public bool CanBeUsed(PickableItem item)
        {
            foreach (var usable in _usableItems.itemList)
            {
                if (item.itemType == usable.itemType)
                {
                    return true;
                }
            }
            return false;
        }

        private void Use(PickableItem item)
        {
            GetComponent<IInteraction>().Act();
            DisableUI();
        }

        public GameObject GetObject()
        {
            return gameObject;
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
            _interactUI.gameObject.SetActive(false);
        }
        
        public void EnableUI()
        {
            _interactUI.gameObject.SetActive(true);
        }
    }
}