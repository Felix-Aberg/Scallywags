using UnityEngine;
using DG.Tweening;


namespace ScallyWags
{
    public class FixableInteraction : MonoBehaviour, IInteraction
    {
        [SerializeField] bool scalesWithRepair;
        private ShipCondition _shipCondition;
        private float _timer;
        private float _delay = 15f;

        private ParticleSystem[] _particleSystems;
    
        private int charges = 0;
        private int chargesRequired = 5;
        
        private int damageDone;
        public void Start()
        {
            _shipCondition = GetComponentInParent<ShipCondition>();
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        void Update()
        {
            if (_shipCondition == null) return;

            _timer += Time.deltaTime;

            if (_timer > _delay)
            {
                _timer = 0;
                _shipCondition.TakeDamage();
                damageDone++;
            }
        }

        public void Act()
        {
            charges++;
            foreach (var p in _particleSystems)
            {
                p.Play();
            }

            if (scalesWithRepair)
            {
                var newScale = new Vector3(transform.localScale.x, Mathf.Max(transform.localScale.y - 0.2f, 0.3f), transform.localScale.z);

                transform.DOScale(newScale, 0.2f);
            }
            
            if (charges >= chargesRequired)
            {
                Fix();
            }
        }

        private void Fix()
        {
            EventManager.TriggerEvent("IntroDone", new EventManager.EventMessage(null));
            charges = 0;
            _shipCondition.FixDamage(1);
            gameObject.SetActive(false);
        }
    }
}