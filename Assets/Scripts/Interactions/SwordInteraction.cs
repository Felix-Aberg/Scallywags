using UnityEngine;

namespace ScallyWags
{
    public class SwordInteraction : MonoBehaviour, IInteraction 
    {
        public void Act()
        {
            GetComponent<IDamageable>().TakeDamage();
        }
    }
}
