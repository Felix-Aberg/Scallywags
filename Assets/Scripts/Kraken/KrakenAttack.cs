using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenAttack : MonoBehaviour
{
    private BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        var destroyableChild = other.gameObject.GetComponentInChildren<IDamageable>();
        
        destroyableChild?.TakeDamage();
        
        var destroyable = other.gameObject.GetComponent<IDamageable>();
        
        destroyable?.TakeDamage();
        _collider.enabled = false;
    }
}
