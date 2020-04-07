using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScallyWags
{
    public class HoleInteraction : MonoBehaviour, IInteraction
    {
        private int hammerHits = 0;
        private int hitsRequired = 5;
        public void Act()
        {
            // TODO hammer sounds effects etc
            hammerHits++;

            if (hammerHits >= hitsRequired)
            {
                Fix();
            }
        }

        private void Fix()
        {
            Destroy(gameObject);
        }
    }
}
