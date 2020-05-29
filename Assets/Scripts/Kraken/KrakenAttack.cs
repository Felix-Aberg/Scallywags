using System;
using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class KrakenAttack : MonoBehaviour
{
    private BoxCollider[] _colliders;
    private float _hitForce = 100f;
    private string _eventName = "KrakenSlams";
    private bool _disabled;
    
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
        _disabled = false;
    }

    public void DisableCollider()
    {
        _disabled = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_disabled)
        {
            return;
        }
        
        var destroyable = other.gameObject.GetComponent<IDamageable>();

        if (destroyable != null)
        {
            EventManager.TriggerEvent(_eventName, null);
            destroyable.TakeDamage(transform.position, _hitForce);
        }
        else
        {
            var rb = other.gameObject.GetComponent<Rigidbody>();
            var dir = other.transform.position - transform.position;
            rb?.AddForce(dir * _hitForce, ForceMode.Impulse);
        }

        var player = other.gameObject.GetComponent<Player>();
        player?.TakeDamage(transform.position, _hitForce);
    }
}
