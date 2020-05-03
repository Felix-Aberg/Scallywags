using UnityEngine;

namespace ScallyWags
{
    [System.Serializable]
    public class ShipHealth
    {
        private int _maxHealth;
        [SerializeField] private int _health;

        public ShipHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
        }
        
        public void FixDamage(int value)
        {
            if (_health < 0) return;
            var health = _health + value;
            _health = Mathf.Clamp(health, 0, _maxHealth);
        }

        public void TakeDamage(int value)
        {
            var health = _health - value;
            _health = Mathf.Clamp(health, 0, _maxHealth);
        }

        public int GetHealth()
        {
            return _health;
        }

        public int GetMissingHealth()
        {
            return _maxHealth - _health;
        }
    }
}