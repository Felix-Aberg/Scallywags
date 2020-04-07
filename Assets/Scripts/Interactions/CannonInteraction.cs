using System;
using UnityEngine;

namespace ScallyWags 
{
    public class CannonInteraction : MonoBehaviour, IInteraction
    {
        public void Act()
        {
            // Shoot cannonball
            // Visual Effects
            Debug.Log("Cannon shoots: boom!");
        }
    }
}