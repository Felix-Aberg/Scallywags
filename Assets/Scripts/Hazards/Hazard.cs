using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public abstract class Hazard : MonoBehaviour
    {
        public abstract void Execute(HazardData hazard);
    }
}
