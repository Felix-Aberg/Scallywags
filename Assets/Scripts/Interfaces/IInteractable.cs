using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// Adding this to item will allow players to use items on it
    /// </summary>
    public interface IInteractable
    {
        void Interact(PickableItem item, Player player);
    }
}