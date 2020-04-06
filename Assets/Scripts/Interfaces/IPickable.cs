using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// Adding this to item will allow players to pick it up
    /// </summary>
    public interface IPickable
    {
        IPickable Pickup(Player player);
        void Drop();
        bool IsAvailable();
        GameObject GetObject();
    }
}