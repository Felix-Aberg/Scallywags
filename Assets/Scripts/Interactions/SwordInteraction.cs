using UnityEngine;

namespace ScallyWags
{
    public class SwordInteraction : MonoBehaviour, IInteraction
    {
        private float _hitForce = 2f;
        public void Act()
        {
            GetComponent<IDamageable>().TakeDamage(transform.position, _hitForce);
        }
    }
}
