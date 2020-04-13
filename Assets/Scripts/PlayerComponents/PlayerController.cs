using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// Holds player movement logic
    /// </summary>
    public class PlayerController
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _turnSpeed = 20f;
        private Vector3 _lastDir = new Vector3();
        private Quaternion rot;

        void Init()
        {
        }

        public void Tick(Transform player, float horizontal, float vertical)
        {
            // Tempcontrols
            var moveDir = new Vector3(horizontal, 0, vertical);
            
            if (moveDir.sqrMagnitude > 0);
            {
                var movement = _speed * Time.deltaTime * moveDir;
                player.transform.position += movement;
                _lastDir = moveDir;
            }

            if (_lastDir.sqrMagnitude > 0)
            {
                rot = Quaternion.LookRotation(_lastDir);
                player.rotation = Quaternion.RotateTowards(player.transform.rotation, rot, _turnSpeed);
            }
        }
    }
}
