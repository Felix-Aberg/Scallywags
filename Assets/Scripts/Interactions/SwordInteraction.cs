using UnityEngine;

namespace ScallyWags
{
    public class SwordInteraction : MonoBehaviour, IInteraction
    {
        private float _hitForce = 20f;
        public void Act()
        {
            EventManager.TriggerEvent("IntroDone", new EventManager.EventMessage(null));
            GetComponent<IDamageable>().TakeDamage(transform.position, _hitForce);
        }
    }
}
