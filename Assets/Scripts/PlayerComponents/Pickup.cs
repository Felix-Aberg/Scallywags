/// <summary>
/// Pickup/drop logic for IPickable items
/// </summary>
public class Pickup
{
    private IPickable _pickedUpItem;
    public bool TryToPickUp(IPickable itemToPickUp)
    {
        return _pickedUpItem == null && itemToPickUp.IsAvailable();
    }
    
    public void PickUp(IPickable item, Player player)
    {
        if (_pickedUpItem == null)
        {
            _pickedUpItem = item.Pickup(player);
        }
    }
    
    public bool TryToDrop()
    {
        return _pickedUpItem != null;
    }
    public void Drop()
    {
        if(_pickedUpItem == null) return;
        
        _pickedUpItem.Drop();
        _pickedUpItem = null;
    }
}
