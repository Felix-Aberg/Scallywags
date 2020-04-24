using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ScallyWags
{
    public class InputHandler
    {
        private string horizontal = "Horizontal";
        private string vertical = "Vertical";
        private string pickup = "Pickup";
        private string interact = "Interact";
        private string jump = "Jump";
        private string yarr = "Yarr";

        public Inputs GetInputs(int index)
        {
            if (index < 1 || index > 4)
            {
                Debug.LogError("Array out of index in InputHandler. Array ranges from 1-4. Index: " + index);
            }
            Inputs inputs;

            inputs.horizontal = GetXAxis(index);
            inputs.vertical = GetYAxis(index);
            inputs.pickUpPressed = GetPickup(index);
            inputs.pickUpDown = GetPickupDown(index);
            inputs.pickUpReleased = GetPickupUp(index);
            inputs.interActPressed = GetInteractDown(index);
            inputs.jumpPressed = GetJumpDown(index);

            return inputs;
        }
        
        private float GetXAxis(int index)
        {
            return Input.GetAxis(horizontal + index);
        }

        private float GetYAxis(int index)
        {
            return Input.GetAxis(vertical + index);
        }

        private bool GetPickupDown(int index)
        {
            return Input.GetButtonDown(pickup + index);
        }

        private bool GetPickup(int index)
        {
            return Input.GetButton(pickup + index);
        }

        private bool GetPickupUp(int index)
        {
            return Input.GetButtonUp(pickup + index);
        }

        private bool GetInteractDown(int index)
        {
            return Input.GetButtonDown(interact + index);
        }

        private bool GetJumpDown(int index)
        {
            return Input.GetButtonDown(jump + index);
        }

        private bool GetYarrDown(int index)
        {
            return Input.GetButtonDown(yarr + index);
        }
    }
}
