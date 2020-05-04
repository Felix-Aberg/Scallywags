using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class ShipManager : MonoBehaviour
    {
        private List<ShipCondition> ships = new List<ShipCondition>();
        private Transform _spawnPos;

        public void Init()
        {
            var ship = GameObject.FindObjectOfType<ShipCondition>();
            ship.Init(ShipType.Player, this, 20);
            ships.Add(ship);
            _spawnPos = GameObject.FindObjectOfType<EnemyShipSpawn>().gameObject.transform;
        }

        public void Tick()
        {
            foreach (var ship in ships)
            {
                ship.Tick();
            }
        }

        public ShipCondition GetShip(ShipType type)
        {
            foreach (var ship in ships)
            {
                if (ship.ShipType == type)
                {
                    return ship;
                }
            }
            return null;
        }

        private void OnEnable()
        {
            EventManager.StartListening("EnemyShip", CreateShip);
        }
        
        private void OnDisable()
        {
            EventManager.StopListening("EnemyShip", CreateShip);
        }
        private void CreateShip(EventManager.EventMessage message)
        {
            CreateShipObject(message.HazardData.Prefab, ShipType.Enemy, message.HazardData.Health);
        }

        private void CreateShipObject(GameObject prefab, ShipType shipType, int health)
        {
            foreach (var s in ships)
            {
                if (s.ShipType == shipType)
                {
                    return;
                }
            }
            
            var go = GameObject.Instantiate(prefab, _spawnPos.position, Quaternion.identity);
            go.transform.rotation = Quaternion.Euler(0, 180, 0);
            var ship = go.GetComponent<ShipCondition>();
            ship.Init(shipType, this, health);
            ships.Add(ship);
        }

        public void RemoveShip(ShipCondition shipCondition)
        {
            if (ships.Contains(shipCondition))
            {
                ships.Remove(shipCondition);
            }
        }
    }
}
