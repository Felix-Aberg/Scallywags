using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class RailingInteraction : MonoBehaviour, IDamageable
    {
        private ShipCondition _shipCondition;
        private InteractParticles _interactParticles;

        // UI
        private CreateUIElement _createUIElement;
        private FixProgressBar _bar;

        // Timers
        private float _fixingTime = 5f;
        private float _fixingTimer = 0f;
        private float _scaleSpeed = 0.35f;

        [SerializeField] private GameObject _fixableShipPart;
        [SerializeField] private GameObject _particleSystem;

        public bool Broken => _broken;
        private bool _broken;   

        void Start()
        {
            _shipCondition = GetComponentInParent<ShipCondition>();
            _interactParticles = GetComponentInChildren<InteractParticles>();
            _createUIElement = FindObjectOfType<CreateUIElement>();
            if (_createUIElement)
            {
                var go = _createUIElement.CreateElement(UIElement.ProgressBar, transform.position);
                _bar = go.GetComponent<FixProgressBar>();
                _bar.Init(0, _fixingTime, transform.position);
                _bar.gameObject.SetActive(false);
            }
        }

        public void Act()
        {
            if (_broken == false) return;
            
            _interactParticles.Play();

            _bar.gameObject.SetActive(true);
            _bar.UpdateValues(_fixingTimer, _fixingTime);

            _fixingTimer += Time.deltaTime;
            if (_fixingTimer >= _fixingTime)
            {
                Fix();
                Destroy(_bar.gameObject);
            }
        }

        private void Fix()
        {
            EventManager.TriggerEvent("IntroDone", new EventManager.EventMessage(null));
            _shipCondition.FixDamage(1);
            
            _fixableShipPart.SetActive(true);
            _broken = false;
        }

        public void TakeDamage()
        {
            if (_broken)
            {
                return;
            }
            
            _broken = true;
            _shipCondition.TakeDamage();

            if (_particleSystem == null)
            {
                Debug.LogError(gameObject.name + " missing particle system");
            }
            else
            {
                Instantiate(_particleSystem, transform.position, Quaternion.identity);
            }

            _fixableShipPart.SetActive(false);
        }

        public void TakeDamage(Vector3 hitDir, float hitForce)
        {
            TakeDamage();
        }

        public Vector3 GetPos()
        {
            return transform.position;
        }
    }
}