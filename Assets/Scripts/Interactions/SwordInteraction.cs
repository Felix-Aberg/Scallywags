using UnityEngine;

namespace ScallyWags
{
    public class SwordInteraction : MonoBehaviour, IInteraction
    {
        private float _hitForce = 10f;
        public void Act()
        {
            GetComponent<IDamageable>().TakeDamage(transform.position, _hitForce);
        }
    }
}
