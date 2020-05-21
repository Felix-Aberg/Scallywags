using UnityEngine;

namespace ScallyWags
{
    public class FixableInteraction : MonoBehaviour, IInteraction
    {

        private ShipCondition _shipCondition;
        private float _timer;
        private float _delay = 30f;

        private InteractParticles _interactParticles;
        
        // UI
        private CreateUIElement _createUIElement;
        private FixProgressBar _bar;

        // Timers
        private float _fixingTime = 5f;
        private float _fixingTimer = 0f;
        private float _scaleSpeed = 0.35f;

        [Header("Fire")] 
        [SerializeField] bool scalesWithRepair;


        [Header("Audio")]
        [SerializeField] private SimpleAudioEvent _audioEvent;
        private AudioSource _audioSource;

        void Start()
        {
            _shipCondition = GetComponentInParent<ShipCondition>();
            _interactParticles = GetComponentInChildren<InteractParticles>();
            _createUIElement = FindObjectOfType<CreateUIElement>();
            var go = _createUIElement.CreateElement(UIElement.ProgressBar, transform.position);
            _bar = go.GetComponent<FixProgressBar>();
            _bar.Init(0, _fixingTime, transform.position);
            _bar.gameObject.SetActive(false);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = _audioEvent.MixerGroup;

            CreateProgressBar();
        }

        void Update()
        {
            if (_shipCondition == null) return;

            _timer += Time.deltaTime;

            if (_timer > _delay)
            {
                _timer = 0;
                _shipCondition.TakeDamage(1);
            }
        }

        public void Act()
        {
            if (!_audioSource.isPlaying)
            {
                _audioEvent?.Play(_audioSource);
            }
            
            _interactParticles.Play();

            ScaleWhenRepaired();
            
            _bar.gameObject.SetActive(true);
            _bar.UpdateValues(_fixingTimer, _fixingTime);

            _fixingTimer += Time.deltaTime;
            if (_fixingTimer >= _fixingTime)
            {
                Fix();
                Destroy(_bar.gameObject);
            }
        }

        private void ScaleWhenRepaired()
        {
            if (scalesWithRepair)
            {
                var y = transform.localScale.y - Time.deltaTime * _scaleSpeed;
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Max(y, 0.2f), transform.localScale.z);
            }
        }

        private void Fix()
        {
            EventManager.TriggerEvent("IntroDone", new EventManager.EventMessage(null));
            _shipCondition.FixDamage(1);
            
            gameObject.SetActive(false);
        }

        private void CreateProgressBar()
        {
            var go = _createUIElement.CreateElement(UIElement.ProgressBar, transform.position);
            _bar = go.GetComponent<FixProgressBar>();
            _bar.Init(0, _fixingTime, transform.position);
            _bar.gameObject.SetActive(false);
        }
    }
}