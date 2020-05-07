using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace ScallyWags
{
    public class MoveShipUp : MonoBehaviour
    {
        private float _delay = 5f;
        private float _rotationDelay = 3f;

        void Start()
        {
            transform.rotation = Quaternion.Euler(0, 180, 35);
            transform.position = new Vector3(0, -20, transform.position.z);
            
            transform.DORotate(new Vector3(0, 180, 0), _rotationDelay);
            transform.DOMoveY(0, _delay).OnComplete(EnableAgents);

        }

        private void EnableAgents()
        {
            var skeletons = GetComponentsInChildren<Skeleton>();

            foreach (var skeleton in skeletons)
            {
                skeleton.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
    }
}
