using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ScallyWags
{
    public class Skeleton : MonoBehaviour, IDamageable, IEntity
    {
        [SerializeField] private Player[] _players;
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


        public void Start()
        {
            _players = FindObjectsOfType<Player>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _navMeshAgent.speed = _normalSpeed;
            _sword = GetComponentInChildren<EnemySword>();
            _navMeshAgent.enabled = false;

            var ragDollColliders = GetComponentsInChildren<CapsuleCollider>();
            var rigidbodyBoxcolliders = GetComponentsInChildren<BoxCollider>();

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.height = 1.756473f;
            capsuleCollider.radius = 0.3f;
            capsuleCollider.center = new Vector3(0, 0.8717635f, 0);

            var ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
            _ragdoll = new Ragdoll(ragDollColliders, ragdollRigidBodies, rigidbodyBoxcolliders, capsuleCollider,
                _animator);
            _ragdoll.DisableRagdoll(ragdollRigidBodies);

            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.mass = 50;
        }

        public void Update()
        {
            if (_isDead) return;
            UpdateAnimations();
            Sense();
            Decide();
            Act();
        }

        public void TakeDamage()
        {
            Die();
        }

        public void TakeDamage(Vector3 hitDir, float hitForce)
        {
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
            throw new NotImplementedException();
        }

        public void Tick()
        {
            throw new NotImplementedException();
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
            if (!_navMeshAgent.enabled) return;
            MoveTowardsPlayer();
            Attack();
        }

        private void Die()
        {
            _isDead = true;
            _navMeshAgent.enabled = false;
            _sword.gameObject.SetActive(false);
        }

        private void GetTarget()
        {
            _players = FindObjectsOfType<Player>();
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

            if (Vector3.Distance(_navMeshAgent.destination, _targetPlayer.transform.position) > 1)
            {
                if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.ResetPath();
                }
            }

            if (_navMeshAgent.hasPath || _navMeshAgent.pathPending) return;

            _navMeshAgent.SetDestination(_targetPlayer.transform.position);
        }

        private void Attack()
        {
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
        }
    }
}
