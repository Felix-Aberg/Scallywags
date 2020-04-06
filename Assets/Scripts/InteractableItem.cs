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

        public void Interact(PickableItem item, Player player)
        {
            foreach (var usable in _usableItems.itemList)
            {
                if (item.GetType() == usable.GetType())
                {
                    player.Drop();
                    Use(item);
                }
            }
        }

        private void Use(PickableItem item)
        {
            GetComponent<IInteraction>().Act();
            Destroy(item.gameObject);
        }
    }
}