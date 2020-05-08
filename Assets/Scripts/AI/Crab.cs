using Cinemachine;
using DG.Tweening;
using ScallyWags;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

public class Crab : MonoBehaviour, IEntity, IDamageable
{
    [SerializeField] private ScoreItem[] _treasure;
    private NavMeshAgent _navMeshAgent;
    private Pickup _pickup;
    [SerializeField] ScoreItem _targetItem;
    [SerializeField] private PickableItem _pickedUpItem;
    private bool _isDead;
    private Vector3 _startPos;

    private Animator _animator;
    private Rigidbody _rigidBody;
    
    private float _normalSpeed = 4f;
    private float _carrySpeed = 1.5f;
    private bool _died;
    

    public void Init(int index = 0)
    {
        _treasure = FindObjectsOfType<ScoreItem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.isKinematic = true;
        _rigidBody.useGravity = false;
        _startPos = transform.position;
        _navMeshAgent.speed = _normalSpeed;
    }

    public void Tick()
    {
        if (_isDead) return;
        Sense();
        Decide();
        Act();
    }
    
    public bool IsDead()
    {
        return _isDead;
    }
    private void UpdateAnimations()
    {
        _animator.SetBool("Grounded", true);
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        if (_pickedUpItem != null)
        {
            _animator.SetBool("Carrying", true);
            _animator.SetLayerWeight(1, 1);
        }
        else
        {
            _animator.SetBool("Carrying", false);
            _animator.SetLayerWeight(1, 0);
        }
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
    }

    private void Die()
    {
        _isDead = true;
        _navMeshAgent.enabled = false;
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
        if (_pickedUpItem == null && _targetItem == null) return;
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
                _pickedUpItem.transform.localPosition = Vector3.zero;
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
        if (_isDead) return;
        Drop();
        Die();
        Push();
    }
    
    public void TakeDamage(Vector3 hitDir, float hitForce)
    {
        if (_isDead) return;
        Drop();
        Die();
        Push();
        var pickable = gameObject.AddComponent<PickableItem>();
        pickable.itemType = ItemType.Crab;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    private void Push()
    {
        _rigidBody.DORotate(new Vector3(0, 0, -180), 1f);
        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = true;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
    }
}
