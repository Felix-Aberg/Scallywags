﻿using ScallyWags;
using UnityEngine;
using UnityEngine.AI;

public class Crab : MonoBehaviour, IEntity, IDamageable
{
    [SerializeField] private ScoreItem[] _treasure;
    private NavMeshAgent _navMeshAgent;
    private Pickup _pickup;
    [SerializeField] ScoreItem _targetItem;
    [SerializeField] private PickableItem _pickedUpItem;
    private bool _isDead;
    private Vector3 _startPos;
    
    private float _normalSpeed = 4f;
    private float _carrySpeed = 2f;

    public void Init(int index = 0)
    {
        _treasure = FindObjectsOfType<ScoreItem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _startPos = transform.position;
        _navMeshAgent.speed = _normalSpeed;
    }

    public void Tick()
    {
        Sense();
        Decide();
        Act();
    }

    private void Sense()
    {
        if (_targetItem?.GetComponent<PickableItem>().PickedUpBy != null)
        {
            _targetItem = null;
        }
        
        if (_pickedUpItem != null && Vector3.Distance(_startPos, transform.position) < 2)
        {
            _isDead = true;
        }
    }
    
    private void Decide()
    {
        if (_targetItem == null)
        {
            GetClosestScoreItem();
        }
    }

    private void Act()
    {
        GoForTreasure();
        TryPickingUp();
        ReturnToSea();
        Die();
    }

    private void Die()
    {
        if (_isDead)
        {
            _navMeshAgent.ResetPath();
            Drop();
            gameObject.SetActive(false);
        }
    }

    private void GetClosestScoreItem()
    {
        _treasure = FindObjectsOfType<ScoreItem>();
        var distance = float.MaxValue;
        foreach (var t in _treasure)
        {
            var d = Vector3.Distance(transform.position, t.transform.position);
            if (d < distance)
            {
                distance = d;
                _targetItem = t;
            }
        }
    }

    private void GoForTreasure()
    {
        if (_pickedUpItem != null) return;

        if (Vector3.Distance(_navMeshAgent.destination, _targetItem.transform.position) > 1)
        {
            _navMeshAgent.ResetPath();
        }
        
        if (_navMeshAgent.hasPath || _navMeshAgent.pathPending) return;
        _navMeshAgent.SetDestination(_targetItem.transform.position);
    }

    private void TryPickingUp()
    {
        if (_pickedUpItem != null) return;
        if (Vector3.Distance(transform.position, _targetItem.transform.position) < 2)
        {
            HandlePickingUp();
        }
    }
    
    private void ReturnToSea()
    {
        if (_pickedUpItem == null) return;
        if(_navMeshAgent.hasPath || _navMeshAgent.pathPending) return;
        _navMeshAgent.SetDestination(_startPos);
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    private void HandlePickingUp()
    {
        if (_targetItem == null) return;

        if (_pickedUpItem == null)
        {
            var item = _targetItem.GetComponent<IPickable>();
            if (item.IsAvailable())
            {
                _pickedUpItem = item.Pickup(this) as PickableItem;
                _navMeshAgent.speed = _carrySpeed;
                _navMeshAgent.ResetPath();
                _navMeshAgent.SetDestination(_startPos);
            }
        }
    }

    private void Drop()
    {
        if(_pickedUpItem == null) return;
        _pickedUpItem.Drop();
        _pickedUpItem = null;
    }

    public void TakeDamage()
    {
        _isDead = true;
    }
}