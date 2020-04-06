using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    IPickable Pickup(Player player);
    void Drop();
    bool IsAvailable();
}
