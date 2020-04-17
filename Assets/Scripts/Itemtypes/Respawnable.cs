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
    
        // Start is called before the first frame update
        void Start()
        {
            _startPos = transform.position;
            _startRot = transform.rotation;
            _rb = GetComponent<Rigidbody>();
        }
    
        // Update is called once per frame
        void Update()
        {
            if (transform.position.y < _respawnLimit)
            {
                Respawn();
            }
        }
    
        public void Respawn()
        {
            transform.position = _startPos;
            transform.rotation = _startRot;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }
}
