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

       _playerController = new PlayerController();
    }

    public void Tick()
    {
        _playerController.Tick(transform);
    }
}
