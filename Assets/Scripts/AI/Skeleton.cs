using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ScallyWags
{
    public class Skeleton : MonoBehaviour, IDamageable
    {
        [SerializeField] private Player[] _players;
        private NavMeshAgent _navMeshAgent;
        private Pickup _pickup;
        [SerializeField] Player _targetPlayer;
        [SerializeField] private PickableItem _pickedUpItem;
        private bool _isDead;
        private Animator _animator;
        private float _normalSpeed = 4f;
        private EnemySword _sword;

        public void Start()
        {
            _players = FindObjectsOfType<Player>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _navMeshAgent.speed = _normalSpeed;
            _sword = GetComponentInChildren<EnemySword>();
        }

        public void Update()
        {
            UpdateAnimations();
            Sense();
            Decide();
            Act();
        }
        
        public void TakeDamage()
        {
            _isDead = true;
        }

        public void TakeDamage(Vector3 hitDir, float hitForce)
        {
            _isDead = true;
        }

        public GameObject GetObject()
        {
            return gameObject;
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
            if (_targetPlayer == null)
            {
                GetTarget();
            }
        }

        private void Act()
        {
            MoveTowardsPlayer();
            Attack();
            Die();
        }

        private void Die()
        {
            if (_isDead)
            {
                _navMeshAgent.ResetPath();
                gameObject.SetActive(false);
            }
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
                _navMeshAgent.ResetPath();
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