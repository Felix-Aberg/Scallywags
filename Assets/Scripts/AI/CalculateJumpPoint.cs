using UnityEngine;
using UnityEngine.AI;

namespace ScallyWags
{
    public class CalculateJumpPoint : MonoBehaviour
    {
        private NavMeshLink _navMeshLink;
        private ShipCondition _playerShip;

        private int _shipHealth;

        void Start()
        {
            _navMeshLink = GetComponent<NavMeshLink>();
            var ships = FindObjectsOfType<ShipCondition>();
            foreach (var s in ships)
            {
                if (s.ShipType == ShipType.Player)
                {
                    _playerShip = s;
                }
            }
            _shipHealth = _playerShip.GetHealth();
        }

        void Update()
        {
            if (_playerShip.GetHealth() == _shipHealth) return;
            
            _navMeshLink.endPoint = new Vector3(_navMeshLink.endPoint.x, _playerShip.transform.position.y, _navMeshLink.endPoint.z);
        }
    }
}
