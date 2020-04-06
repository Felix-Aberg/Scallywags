using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

/// <summary>
/// Pickup/drop logic for IPickable items
/// </summary>
public class Pickup
{
    public GameObject _itemNear;
    private IPickable _pickedUpItem;

    public void Tick(Transform transform, Player player)
    {
        if (_pickedUpItem != null)
        {
            _pickedUpItem.GetObject().transform.position = CalculateTargetPos(transform);
        }

        Inputs(player);
    }

    private void Inputs(Player player)
    {
        if (Input.GetButton("Fire2"))
        {
            if (_pickedUpItem != null)
            {
                if (TryToDrop())
                {
                    Drop();
                    Debug.Log("Drop");
                }
            }
        }
        
        if (Input.GetButton("Fire1"))
        {
            var itemToPickUp = _itemNear.GetComponent<IPickable>();
            if (itemToPickUp != null)
            {
                if (TryToPickUp(itemToPickUp))
                {
                    PickUp(itemToPickUp, player);
                    Debug.Log("Pickup");
                }
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
        targetPos.y += 1;
        targetPos.z += 1;
        return targetPos;
    }

    private void PickUp(IPickable item, Player player)
    {
        if (_pickedUpItem == null)
        {
            _pickedUpItem = item.Pickup(player);
        }
    }

    private bool TryToDrop()
    {
        return _pickedUpItem != null;
    }
    private void Drop()
    {
        if(_pickedUpItem == null) return;
        
        _pickedUpItem.Drop();
        _pickedUpItem = null;
    }
}
