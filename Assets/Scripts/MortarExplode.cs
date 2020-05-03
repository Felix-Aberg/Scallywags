using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace ScallyWags
{
    public class MortarExplode : MonoBehaviour
    {
        public GameObject holePrefab;
        public GameObject firePrefab;
        public GameObject particles;
        [SerializeField] private bool causesFire;
        [SerializeField] private bool createsHoles;
        public SimpleAudioEvent _audio;
        private AudioSourcePoolManager _audioPool;

        private void Start()
        {
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();
        }

        private void OnCollisionEnter(Collision other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable == null)
            {
                damageable = other.gameObject.GetComponentInParent<IDamageable>();
            }

            damageable?.TakeDamage();

            var particleSystem = Instantiate(particles, transform.position, Quaternion.identity);
            if (particleSystem == null)
            {
                Debug.LogError("Missing particle system prefab");
            }
            else
            {
                var systems = particleSystem.GetComponentsInChildren<ParticleSystem>();
                foreach (var s in systems)
                {
                    s.Play();
                }
            }
            _audioPool.PlayAudioEvent(_audio, transform.position);

            var ship = other.gameObject.GetComponentInParent<ShipCondition>();

            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
            {
                if (hit.collider.gameObject.GetComponent<InteractableItem>())
                {
                    Destroy(gameObject);
                    return;
                }

                if (hit.collider.gameObject.GetComponentInParent<ShipCondition>())
                {
                    var pos = hit.point;
                    if (createsHoles)
                    {
                        ContactPoint contact = other.contacts[0];
                        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
                        var hole = Instantiate(holePrefab, pos, rotation);
                        hole.transform.SetParent(hit.transform);
                        hole.transform.localRotation = Quaternion.identity;
                        ship?.TakeDamage();
                    }
                    if (causesFire)
                    {
                        var fire = Instantiate(firePrefab, pos, Quaternion.identity);
                        fire.transform.SetParent(hit.transform);
                        fire.transform.localRotation = Quaternion.identity;
                    }
                }
            }
            
            Destroy(gameObject);
        }
    }
}