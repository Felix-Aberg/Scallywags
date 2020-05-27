using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScallyWags
{
    public class Skeleton : MonoBehaviour, IDamageable, IEntity
    {
        public EntityManager EntityManager;
        
        [SerializeField] private List<Player> _players = new List<Player>();
        private NavMeshAgent _navMeshAgent;
        private Pickup _pickup;
        [SerializeField] private Player _targetPlayer;
        [SerializeField] private PickableItem _pickedUpItem;
        private bool _isDead;
        private Animator _animator;
        private float _normalSpeed = 4f;
        private EnemySword _sword;
        private Ragdoll _ragdoll;
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;
        private AudioSourcePoolManager _audioSourcePoolManager;
        private string _enemyDamageEventName = "EnemyDamage";
        
        // Sword
        private float _attackTimer;
        private float _attackDelay = 1f;
        
        private float _damageTimer;
        private float _damageDelay = 1f;
        
        private float _hitForce = 40f;
        private ParticleSystem _slash;


        public void Start()
        {
            _sword = GetComponentInChildren<EnemySword>();
            _animator = GetComponent<Animator>();

            var ragDollColliders = GetComponentsInChildren<CapsuleCollider>();
            var rigidbodyBoxcolliders = GetComponentsInChildren<BoxCollider>();
            _slash = GetComponentInChildren<ParticleSystem>();

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.height = 1.756473f;
            capsuleCollider.radius = 0.3f;
            capsuleCollider.center = new Vector3(0, 0.8717635f, 0);

            var ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
            _ragdoll = new Ragdoll(ragDollColliders, ragdollRigidBodies, rigidbodyBoxcolliders, capsuleCollider,
                _animator);
            _ragdoll.DisableRagdoll(ragdollRigidBodies);

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _normalSpeed;
            _navMeshAgent.enabled = true;
            EntityManager = FindObjectOfType<EntityManager>();
        }

        public void Update()
        {
            if (!_navMeshAgent.isOnNavMesh)
            {
                if( NavMesh.SamplePosition(transform.position, out var closestHit, 50, 1 )){
                    transform.position = closestHit.position;
                }
            }
            
            if (_isDead) return;
            
            _attackTimer += Time.deltaTime;
            _damageTimer += Time.deltaTime;
            
            UpdateAnimations();
            Sense();
            Decide();
            Act();
        }

        public void TakeDamage()
        {
            if (_isDead) return;
            Die();
        }

        public void TakeDamage(Vector3 hitDir, float hitForce)
        {
            if (_isDead) return;
            Die();
            var dir = transform.position - _targetPlayer.transform.position;
            _ragdoll.EnableRagdoll(dir.normalized, hitForce);
        }

        public Vector3 GetPos()
        {
            return transform.position;
        }

        public void Init(int index = 0)
        {
        }

        public void Tick()
        {
        }

        public GameObject GetObject()
        {
            return gameObject;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        private void UpdateAnimations()
        {
            _animator.SetBool("Grounded", true);
            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
            _animator.SetBool("Carrying", false);
            _animator.SetLayerWeight(1, 1);
        }

        private void Sense()
        {
        }

        private void Decide()
        {
            GetTarget();
        }

        private void Act()
        {
            MoveTowardsPlayer();
            Attack();
        }

        private void Die()
        {
            _isDead = true;
            _navMeshAgent.enabled = false;
            _sword.gameObject.SetActive(false);
            EventManager.TriggerEvent(_enemyDamageEventName, null);
            var pickable = gameObject.AddComponent<PickableItem>();
            pickable.itemType = ItemType.Skeleton;
        }

        private void GetTarget()
        {
            _players = EntityManager.GetAllPlayers();
            var distance = float.MaxValue;
            foreach (var t in _players)
            {
                var d = Vector3.Distance(transform.position, t.transform.position);
                if (d < distance)
                {
                    distance = d;
                    _targetPlayer = t;
                }
            }
        }

        private void MoveTowardsPlayer()
        {
            if (_pickedUpItem != null) return;

            if (_navMeshAgent.pathPending) return;

            if (Vector3.Distance(_navMeshAgent.destination, _targetPlayer.transform.position) < 1)
            {
                return;
            }
            
            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.SetDestination(_targetPlayer.transform.position);
            }
        }

        private void Attack()
        {
            if (_attackTimer < _attackDelay)
            {
                return;
            }

            _attackTimer = 0;
            
            if (Vector3.Distance(transform.position, _targetPlayer.transform.position) < 1)
            {
                HandleAttack();
            }
        }

        /// <summary>
        /// Starts animation which activates collider. Dealing damage is handled in the EnemySword.cs
        /// </summary>
        private void HandleAttack()
        {
            if (_targetPlayer == null) return;

            _animator.SetTrigger("Sword");
            _slash.Play();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            var target = other.gameObject.GetComponent<Player>();
            if (target != null)
            {
                if (_damageTimer > _damageDelay)
                {
                    target.TakeDamage(transform.position, _hitForce);
                    _damageTimer = 0;
                    _attackTimer = _attackDelay;
                }
            }
        }
    }
}
