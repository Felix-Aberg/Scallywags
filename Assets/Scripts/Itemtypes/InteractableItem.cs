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
                if (item.itemType == usable.itemType)
                {
                    Use(item);
                    if (item.singleUse)
                    {
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
            }
        }

        private void Use(PickableItem item)
        {
            GetComponent<IInteraction>().Act();
        }
    }
}