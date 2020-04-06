using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script constructs the player
/// </summary>
public class Player : MonoBehaviour, IEntity
{
    public int Index => _index;
    [SerializeField] private int _index;
    private PlayerController _playerController;
    private Pickup _pickup;
    
    // Monobehaviors
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    
    public void Init(int index)
    {
        _index = index;
       _capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
       _rigidbody = gameObject.AddComponent<Rigidbody>();
       _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

       _pickup = new Pickup();
       _playerController = new PlayerController();
    }

    public void Tick()
    {
        _playerController.Tick(transform);
        _pickup.Tick(gameObject.transform, this);
    }

    private void OnTriggerStay(Collider other)
    {
        _pickup._itemNear = other.gameObject;
    }
}
