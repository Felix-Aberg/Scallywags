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
        [SerializeField] private float _deadZone = 0.4f;
        private Vector3 _lastDir = new Vector3();
        private Quaternion rot;

        public void Init()
        {
        }

        public void Tick(Transform player, float horizontal, float vertical)
        {
            Rigidbody _rb = player.GetComponent<Rigidbody>();

            // Tempcontrols
            var moveDir = new Vector3(horizontal, 0, vertical);
            if (moveDir.magnitude > _deadZone)
            {
                // Clamp input movespeed
                if (moveDir.magnitude > 1f)
                    moveDir.Normalize();

                // Execute movement

                //var movement = _speed * Time.deltaTime * moveDir;
                //player.transform.position += movement;

                _lastDir = moveDir;
            }
            else
            {
                moveDir = Vector3.zero;
            }

            Vector3 movement = _rb.velocity;
            movement.x = _speed * moveDir.x;
            movement.z = _speed * moveDir.z;
            movement.y = _rb.velocity.y;
            _rb.velocity = movement;

            if (_lastDir != Vector3.zero)
            {
                rot = Quaternion.LookRotation(_lastDir);
            }

            player.rotation = Quaternion.RotateTowards(player.transform.rotation, rot, _turnSpeed);

        }
    }
}
