using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class InputHandler : MonoBehaviour
    {
        public float GetXAxis(int index)
        {
            if (index < 1 || index > 4)
            {
                Debug.LogError("Array out of index in InputHandler GetXAxis. Array ranges from 1-4. Index: " + index);
                return 0;
            }
            else
            {
                if (index == 2)
                    Debug.Log(Input.GetAxis("Horizontal" + index));
                return Input.GetAxis("Horizontal" + index);
            }

        }

        public float GetYAxis(int index)
        {
            if (index < 1 || index > 4)
            {
                return 0;
            }
            else
            {
                return Input.GetAxis("Vertical" + index);
            }
        }

        public bool GetPickup(int index)
        {
            if (index < 1 || index > 4)
            {
                Debug.LogError("Array out of index in InputHandler GetPickup. Array ranges from 1-4. Index: " + index);
                return false;
            }
            else
            {
                return Input.GetButton("Pickup" + index);
            }
        }

        public bool GetInteract(int index)
        {
            if (index < 1 || index > 4)
            {
                Debug.LogError("Array out of index in InputHandler GetInteract. Array ranges from 1-4. Index: " + index);
                return false;
            }
            else
            {
                return Input.GetButton("Interact" + index);
            }
        }

        public bool GetJump(int index)
        {
            if (index < 1 || index > 4)
            {
                Debug.LogError("Array out of index in InputHandler GetJump. Array ranges from 1-4. Index: " + index);
                return false;
            }
            else
            {
                return Input.GetButton("Jump" + index);
            }
        }

        public bool GetYarr(int index)
        {
            if (index < 1 || index > 4)
            {
                Debug.LogError("Array out of index in InputHandler GetYarr. Array ranges from 1-4. Index: " + index);
                return false;
            }
            else
            {
                return Input.GetButton("Yarr" + index);
            }
        }
    }
}
