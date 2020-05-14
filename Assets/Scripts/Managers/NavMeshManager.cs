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
            _navMeshSurface = FindObjectOfType<NavMeshSurface>();
            _navMeshSurface.BuildNavMesh();
        }
        
        public void UpdateMesh()
        {
            _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
        }
    }
}