using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ScallyWags
{
    public class NavMeshManager : MonoBehaviour
    {
        private NavMeshSurface _navMeshSurface;
        private float y;
        private ShipCondition _ship;

        public void Init(ShipCondition ship)
        {
            _ship = ship;
            _navMeshSurface = GetComponent<NavMeshSurface>();
        }
        
        public void UpdateMesh()
        {
            var data = _navMeshSurface.navMeshData;
            var y = _ship.transform.position.y;
            data.position = new Vector3(data.position.x, y, data.position.z);
            _navMeshSurface.UpdateNavMesh(data);
        }
    }
}