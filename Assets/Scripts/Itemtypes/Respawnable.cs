using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace ScallyWags
{
    public class Respawnable : MonoBehaviour, IRespawnable
    {
        private Vector3 _startPos;
        private Quaternion _startRot;
        private float _respawnLimit = -50f;
        private Rigidbody _rb;
        private LevelEventManager _levelEventManager;
    
        // Start is called before the first frame update
        void Start()
        {
            _startPos = transform.position;
            _startRot = transform.rotation;
            _rb = GetComponent<Rigidbody>();
            _levelEventManager = GameObject.FindObjectOfType<LevelEventManager>();
                
        }
    
        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < _respawnLimit)
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
            var player = GetComponent<Player>();
            if (player)
            {
                player.Respawn();
            }
            transform.position = _startPos;
            transform.rotation = _startRot;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}
