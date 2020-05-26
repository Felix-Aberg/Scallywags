using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class Respawnable : MonoBehaviour, IRespawnable
    {
        private Vector3 _startPos;
        private Quaternion _startRot;
        private float _respawnLimit = -50f;
        private Rigidbody _rb;
        private LevelEventManager _levelEventManager;
        private ShipManager _shipManager;
        private float _respawnTimer;
        private float _respawnDelay = 5;

        // Start is called before the first frame update
        void Start()
        {
            _startPos = transform.position;
            _startRot = transform.rotation;
            _rb = GetComponent<Rigidbody>();
            _levelEventManager = GameObject.FindObjectOfType<LevelEventManager>();
            _shipManager = FindObjectOfType<ShipManager>();
            Debug.Log(_shipManager);
            Debug.Log(!!_shipManager);
        }
    
        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < _respawnLimit || Vector3.Distance(transform.position, _startPos) > 50)
            {
                if (GetComponent<Player>())
                {
                    _levelEventManager.IncrementDeaths();
                }
                Respawn();
            }
        }
    
        public void Respawn()
        {
            // Only respawn items/players if the player ship isn't sinking
            if (!_shipManager.GetShip(ShipType.Player).IsSinking())
            {
                var player = GetComponent<Player>();
                if (player)
                {
                    player.Respawn();
                    _respawnTimer = 0;
                }
                transform.position = _startPos;
                transform.rotation = _startRot;
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                gameObject.SetActive(true);
            }
        }

        public void Tick()
        {
            _respawnTimer += Time.deltaTime;

            if (_respawnTimer > _respawnDelay)
            {
                Respawn();
            }
        }

        // Used for cannon balls to not spawn immediately
        public void ResetPosition()
        {
            transform.position = _startPos;
            transform.rotation = _startRot;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
