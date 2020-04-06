using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pickup/drop logic for IPickable items
/// </summary>
public class Pickup
{
    private IPickable _pickedUpItem;

    public Pickup(IPickable item, Player player)
    {
        if (_pickedUpItem == null)
        {
            _pickedUpItem = item.Pickup(player);
        }
    }

    public void Drop()
    {
        if(_pickedUpItem == null) return;
        
        _pickedUpItem.Drop();
        _pickedUpItem = null;
    }
}
