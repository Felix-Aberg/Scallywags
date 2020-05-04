using System;
using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class KrakenAttack : MonoBehaviour
{
    private BoxCollider _collider;
    private bool _dealtDamageToShip;
    private bool _dealtDamageToShipPart;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
        _dealtDamageToShip = false;
        _dealtDamageToShipPart = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (_dealtDamageToShipPart == false)
        {
            var destroyable = other.gameObject.GetComponent<IDamageable>();

            if (destroyable != null)
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player)
                {
                    player.Push(transform.position);
                }
                
                destroyable.TakeDamage();
                _dealtDamageToShipPart = true;
            }
        }


        if (_dealtDamageToShip == false)
        {
            var ship = other.gameObject.GetComponent<ShipCondition>();
            if (ship != null)
            {
                ship.TakeDamage(3);
                _dealtDamageToShip = true;
            }
        }
    }
}
