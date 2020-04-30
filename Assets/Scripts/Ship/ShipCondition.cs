using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScallyWags
{
    public class ShipCondition : MonoBehaviour
    {
        public ShipType ShipType => _shipType; 
        [SerializeField] private ShipType _shipType;
        private float _startingDepth;
        private float _sinkingPerDamage;
        private ShipManager _shipManager;
        [SerializeField] ShipHealth _shipHealth;
        private NavMeshManager _navMeshManager;

        // Start is called before the first frame update
        public void Init(ShipType shipType, ShipManager shipManager, int maxHealth = 10)
        {
            _shipHealth = new ShipHealth(maxHealth);
            _shipManager = shipManager;
            _shipType = shipType;

            _sinkingPerDamage = 5f / maxHealth;
            var pos = transform.position;
            transform.position = new Vector3(pos.x, 0, pos.z);
            _startingDepth = transform.position.y;
            _navMeshManager = FindObjectOfType<NavMeshManager>();
        }

        public void Tick()
        {
            if (_shipHealth.GetHealth() < 0)
            {
                Sink();
            }

            if (transform.position.y <= -50)
            {
                _shipManager.RemoveShip(this);
            }
        }

        public void FixDamage(int damage = 1)
        {
            _shipHealth.FixDamage(damage);
            
            var y = transform.position.y + damage;
            y = Mathf.Min(y, _startingDepth);
            transform.DOMoveY(y, 1).OnComplete(_navMeshManager.UpdateMesh);
        }

        public void TakeDamage(int damage = 1)
        {
            if (IsSinking()) return;
            _shipHealth.TakeDamage(damage);

            var depth = _startingDepth - _shipHealth.GetMissingHealth() * _sinkingPerDamage;
            transform.DOMoveY(depth - _sinkingPerDamage * damage, 1).OnComplete(_navMeshManager.UpdateMesh);
        }
        public int GetHealth()
        {
            return _shipHealth.GetHealth();
        }

        public float DepthOffset()
        {
            return _shipHealth.GetMissingHealth() * _sinkingPerDamage;
        }
        
        public bool IsSinking()
        {
            return _shipHealth.GetHealth() <= 0;
        }

        private void Sink()
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, -100, transform.position.z), 0.05f);
        }
    }
}