using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        var destroyableChild = other.gameObject.GetComponentInChildren<IDamageable>();
        
        destroyableChild?.TakeDamage();
        
        var destroyable = other.gameObject.GetComponent<IDamageable>();
        
        destroyable?.TakeDamage();
    }
}
