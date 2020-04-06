using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// Holds player movement logic
    /// </summary>
    public class PlayerController
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _turnSpeed = 10f;

        void Init()
        {
        }

        public void Tick(Transform player, float horizontal, float vertical)
        {
            // Tempcontrols
            var moveDir = new Vector3(horizontal * Time.deltaTime * _speed, 0, vertical * Time.deltaTime * _speed);
            player.transform.position += moveDir;

            if (moveDir != Vector3.zero)
            {
                var rot = Quaternion.LookRotation(moveDir);
                player.rotation = Quaternion.RotateTowards(player.transform.rotation, rot, _turnSpeed);
            }
        }
    }
}
