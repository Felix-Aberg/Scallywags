using System.Collections;
using DG.Tweening;
using ScallyWags;
using UnityEngine;
using UnityEngine.AI;

public class Crab : MonoBehaviour, IEntity, IDamageable
{
    [SerializeField] private SimpleAudioEvent _dieSound;
    private AudioSourcePoolManager _audioSourcePool;
    private ScoreItem[] _treasure;
    private NavMeshAgent _navMeshAgent;
    private Pickup _pickup;
    private ScoreItem _targetItem;
    private PickableItem _pickedUpItem;
    private bool _isDead;
    private Vector3 _startPos;
    private Rigidbody _rigidBody;
    
    private float _normalSpeed = 4f;
    private float _carrySpeed = 1.3f;
    private bool _died;


    public void Init(int index = 0)
    {
        _treasure = FindObjectsOfType<ScoreItem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.isKinematic = true;
        _rigidBody.useGravity = false;
        _startPos = transform.position;
        _navMeshAgent.speed = _normalSpeed;
        _audioSourcePool = FindObjectOfType<AudioSourcePoolManager>();
    }

    public void Tick()
    {
        if (_isDead)
        {
            return;
        }
        Sense();
        Decide();
        Act();
    }
    
    public bool IsDead()
    {
        return _isDead;
    }

    private void Sense()
    {
        if (_pickedUpItem != null && transform.position.y < -9)
        {
            _pickedUpItem.gameObject.SetActive(false);
            Die();
            return;
        }
        
        if (_pickedUpItem && !_pickedUpItem.gameObject.activeInHierarchy)
        {
            _pickedUpItem = null;
        }

        var targetItem = _targetItem?.GetComponent<PickableItem>();
        if (targetItem && targetItem.PickedUpBy != null)
        {
            _targetItem = null;
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
        if (_isDead)
        {
            return;
        }

        if (!_navMeshAgent.isOnNavMesh)
        {
            SetDestinationNearTarget(transform.position);
            return;
        }
        
        GoForTreasure();
        TryPickingUp();
        ReturnToSea();
    }

    private void Die()
    {
        _isDead = true;
        _navMeshAgent.enabled = false;
        StartCoroutine(DisableCrab());
    }

    private IEnumerator DisableCrab()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        DestroyLegSteppers();
    }

    private void GetClosestScoreItem()
    {
        _treasure = FindObjectsOfType<ScoreItem>();
        var distance = float.MaxValue;
        foreach (var t in _treasure)
        {
            if (!t.gameObject.activeInHierarchy)
            {
                continue;
            }

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

        if(_targetItem == null) return;
        
        if (Vector3.Distance(_navMeshAgent.destination, _targetItem.transform.position) > 1)
        {
            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.ResetPath();
            }
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
        Flip();
    }
    
    public void TakeDamage(Vector3 hitDir, float hitForce)
    {
        if (_isDead) return;
        Drop();
        Die();
        Flip();
        _audioSourcePool.PlayAudioEvent(_dieSound);

        //var pickable = gameObject.AddComponent<PickableItem>();
        //pickable.itemType = ItemType.Crab;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    private void Flip()
    {
        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = true;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.DORotate(new Vector3(0, 0, 180), 0.2f);
    }

    private void DestroyLegSteppers()
    {
        var legs = GetComponentsInChildren<LegStepper>();
        foreach (var leg in legs)
        {
            leg.CleanUp();
        }
    }
    
    private void SetDestinationNearTarget(Vector3 pos)
    {
        NavMeshHit hit;
        var repathCount = 50;
        float radius = 0;
        for (int i = 0; i < repathCount; ++i)
        {
            Vector3 randomPosition = Random.insideUnitSphere * radius;
            randomPosition += pos;
            if (NavMesh.SamplePosition(randomPosition, out hit, radius, 1))
            {
                _navMeshAgent.SetDestination(hit.position);
                break;
            }
            else
            {
                ++radius;
            }
        }
    }
}
