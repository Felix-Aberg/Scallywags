﻿using System;
using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class KrakenAttack : MonoBehaviour
{
    private BoxCollider[] _colliders;
    private bool _dealtDamageToShip;
    private bool _dealtDamageToShipPart;
    private float _hitForce = 100f;

    private void Start()
    {
        _colliders = GetComponentsInChildren<BoxCollider>();
        foreach (var collider in _colliders)
        { 
            collider.enabled = true;
        }
    }

    public void EnableCollider()
    {
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
                destroyable.TakeDamage(transform.position, _hitForce);
                if (destroyable as Player == null)
                {
                    _dealtDamageToShipPart = true;
                }
            }
        }
        
        if (_dealtDamageToShip == false)
        {
            var ship = other.gameObject.GetComponentInParent<ShipCondition>();
            if (ship != null)
            {
                ship.TakeDamage(3);
                _dealtDamageToShip = true;
            }
        }
    }
}
