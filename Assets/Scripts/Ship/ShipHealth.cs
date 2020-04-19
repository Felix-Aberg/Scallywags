using UnityEngine;

namespace ScallyWags
{
    public class ShipHealth
    {
        private int _maxHealth;
        private int _health;

        public ShipHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
        }
        
        public void FixDamage(int value)
        {
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
        
        public bool IsSinking()
        {
            return _health <= 0;
        }
    }
}