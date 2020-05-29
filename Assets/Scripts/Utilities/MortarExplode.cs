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
        private float _hitForce = 10f;
        private PositionDecal _decal;

        private void Start()
        {
            _audioPool = FindObjectOfType<AudioSourcePoolManager>();
            _decal = GetComponentInChildren<PositionDecal>();
        }

        private void OnCollisionEnter(Collision other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable == null)
            {
                damageable = other.gameObject.GetComponentInParent<IDamageable>();
            }

            damageable?.TakeDamage(transform.position, _hitForce);

            if (other.gameObject.GetComponent<ScoreItem>())
            {
                return;
            }

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
            
            // Bit shift the index of the layer (11) to get a bit mask
            int layerMask = 1 << 11;

            // This would cast rays only against colliders in layer 11.
            // But instead we want to collide against everything except layer 11. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f, layerMask))
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
                    }

                    if (causesFire)
                    {
                        var fire = Instantiate(firePrefab, pos, Quaternion.identity);
                        fire.transform.SetParent(hit.transform);
                        fire.transform.localRotation = Quaternion.identity;
                    }

                    ship?.TakeDamage();
                }
            }
            
            Destroy(_decal.gameObject);
            Destroy(gameObject);
        }
    }
}