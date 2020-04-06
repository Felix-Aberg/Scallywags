using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adding this to game object makes the item pickable buy player
/// </summary>
public class PickableItem : MonoBehaviour, IPickable
{
    private Player _pickedUpBy;
    public IPickable Pickup(Player player)
    {
        if (_pickedUpBy != null) return null;

        _pickedUpBy = player;
        gameObject.transform.SetParent(player.gameObject.transform);
        return this;
    }

    public void Drop()
    {
        if (_pickedUpBy ==  null) return;
        
        _pickedUpBy = null;
        gameObject.transform.SetParent(null);
    }
}
