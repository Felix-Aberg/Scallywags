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
        [SerializeField] float _jumpForce = 7.2f;
        [SerializeField] private float _gravityModifier = 1.5f;
        JumpCollission _jumpCollission;
        public void Init(Transform transform)
        {
            _jumpCollission = transform.GetComponentInChildren<JumpCollission>();
        }

        public void Tick(Transform transform, Rigidbody rigidBody, bool jumpPressed)
        {
            _jumpCollission.Tick();
            // Initiate jump
            if (jumpPressed && CheckIfGrounded(transform))
            {
                InitiateJump(rigidBody);
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
                Debug.Log("Player IS grounded!");
                return true; // Todo
            }
            Debug.Log("Player ISN'T grounded!");
            return false; // Todo
        }

        
        public void InitiateJump(Rigidbody playerRigidBody)
        {
            Debug.Log("Jump!");
            playerRigidBody.AddForce(0, _jumpForce, 0);

            Vector3 movement = playerRigidBody.velocity;
            movement.x = playerRigidBody.velocity.x;
            movement.z = playerRigidBody.velocity.z;
            movement.y = _jumpForce;

            playerRigidBody.velocity = movement;
        }
    }
}
