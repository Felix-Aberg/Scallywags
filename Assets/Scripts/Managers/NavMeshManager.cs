using UnityEngine;
using UnityEngine.AI;

namespace ScallyWags
{
    public class NavMeshManager : MonoBehaviour
    {
        private NavMeshSurface _navMeshurface;
        private float y;
        private ShipCondition _ship;

        public void Init(ShipCondition ship)
        {
            _ship = ship;
            _navMeshurface = FindObjectOfType<NavMeshSurface>();
            _navMeshurface.BuildNavMesh();
        }
        
        public void UpdateMesh()
        {
            _navMeshurface.BuildNavMesh();
        }
    }
}