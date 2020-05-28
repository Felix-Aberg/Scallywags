using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class PositionDecal : MonoBehaviour
{
    void Start()
    {
        // Bit shift the index of the layer (11 players) to get a bit mask
        int layerMask = 0;
        layerMask |= (1 << LayerMask.NameToLayer("Player"));
        layerMask |= (1 << LayerMask.NameToLayer("Treasure"));

        // This would cast rays only against colliders in layer 11.
        // But instead we want to collide against everything except layer 11. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        
        if(Physics.Raycast(transform.position, Vector3.down, out var hit, 100f, layerMask))
        {
            var pos = hit.point;
            pos.y += 0.2f;
            transform.position = pos;
            if (hit.transform.gameObject.GetComponentInParent<ShipCondition>())
            {
                transform.parent = hit.transform;
            }
        }
    }
}
