using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScallyWags
{
    public class ShipCondition : MonoBehaviour
    {
        public ShipManager.ShipType ShipType => _shipType; 
        [SerializeField] private ShipManager.ShipType _shipType;
        [SerializeField] private int _shipHealth;
        private int _shipMaxHealth = 10;
        private float _startingDepth;
        private float _sinkingPerDamage;
        private ShipManager _shipManager;
        
        // Start is called before the first frame update
        public void Init(ShipManager.ShipType shipType, ShipManager shipManager, int maxHealth = 10)
        {
            _shipManager = shipManager;
            _shipType = shipType;
            _shipMaxHealth = maxHealth;
            _sinkingPerDamage = 5f / _shipMaxHealth;
            var pos = transform.position;
            transform.position = new Vector3(pos.x, 0, pos.z);
            _shipHealth = _shipMaxHealth;
            _startingDepth = transform.position.y;
        }

        public void Tick()
        {
            if (_shipHealth <= 0)
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
            var health = damage + _shipHealth;
            _shipHealth = Mathf.Min(health, _shipHealth);
            
            var y = transform.position.y + damage;
            y = Mathf.Min(y, _startingDepth);
            transform.DOMoveY(y, 1);
        }

        public void TakeDamage()
        {
            _shipHealth -= 1;
            var pos = transform.position;
            transform.DOMoveY(pos.y - _sinkingPerDamage, 1);
        }

        public int GetHealth()
        {
            return _shipHealth;
        }

        private void Sink()
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, -100, transform.position.z), 0.03f);
        }

        public bool IsSinking()
        {
            return _shipHealth <= 0;
        }
    }
}