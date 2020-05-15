using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    /// <summary>
    /// Jump logic for players
    /// </summary>
    [System.Serializable]
    public class Jump
    {
        [SerializeField] public float _jumpForce = 12f;
        [SerializeField] public float _jumpBoost = 20f;
        [SerializeField] public float _gravityModifier = 40f;
        [SerializeField] public float _jumpingGravityModifier = 36f;
        JumpCollission _jumpCollission;
        public void Init(Transform transform)
        {
            _jumpCollission = transform.GetComponentInChildren<JumpCollission>();
        }

        public void Tick(Transform transform, Rigidbody rigidBody, bool jumpPressed, bool jumpHeld)
        {
            _jumpCollission.Tick();
            // Initiate jump
            if (jumpPressed && CheckIfGrounded(transform))
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, _jumpForce, rigidBody.velocity.z);
            }

            if (rigidBody.velocity.y < 0)
            {
                //Apply falling gravity
                rigidBody.AddForce(0f, -_gravityModifier * Time.deltaTime, 0f, ForceMode.VelocityChange);
            }
            else if (rigidBody.velocity.y > 0)
            {
                rigidBody.AddForce(0f, -_jumpingGravityModifier * Time.deltaTime, 0f, ForceMode.VelocityChange);
            }

            if (jumpHeld && rigidBody.velocity.y > 0)
            {
                //Boost jump if holding 
                rigidBody.AddForce(0f, _jumpBoost * Time.deltaTime, 0f, ForceMode.VelocityChange);
            }
            
        }

        public bool IsGrounded()
        {
            return _jumpCollission.grounded;
        }

        // Check if player is in the air or not
        public bool CheckIfGrounded(Transform transform)
        {
            // float groundDistance = 1.02f; //Actually 0.99 but added some margin for error
            bool grounded = _jumpCollission.grounded;
            
            if (grounded)
            {
                return true; // Todo
            }
        return false; // Todo
        }
    }
}
