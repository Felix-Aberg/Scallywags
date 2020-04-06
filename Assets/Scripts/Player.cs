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
    
    public void Init(int index)
    {
        _index = index;
       gameObject.AddComponent<CapsuleCollider>();
       gameObject.AddComponent<Rigidbody>();
       
       _playerController = new PlayerController();
    }

    public void Tick()
    {
        _playerController.Tick();
    }
}
