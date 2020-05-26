using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class ShipManager : MonoBehaviour
    {
        private List<ShipCondition> ships = new List<ShipCondition>();
        private Transform _spawnPos;
        private HazardManager _hazardManager;

        public void Init(HazardManager hazardManager)
        {
            _hazardManager = hazardManager;
            var ship = GameObject.FindObjectOfType<ShipCondition>();
            ship.Init(ShipType.Player, this, 20);
            ships.Add(ship);
            _spawnPos = GameObject.FindObjectOfType<EnemyShipSpawn>().gameObject.transform;
        }

        public void Tick()
        {
            foreach (var ship in ships)
            {
                if (ship.gameObject.activeInHierarchy)
                {
                    ship.Tick();
                }
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
            EventManager.StartListening("SpawnEnemyShip", CreateShip);
            EventManager.StartListening("EnemyShip", EnableShip);
        }
        
        private void OnDisable()
        {
            EventManager.StopListening("SpawnEnemyShip", CreateShip);
            EventManager.StopListening("EnemyShip", EnableShip);
        }
        private void CreateShip(EventManager.EventMessage message)
        {
            CreateShipObject(message.HazardData.Prefab, ShipType.Enemy, message.HazardData.Health);
        }

        private void EnableShip(EventManager.EventMessage message)
        {
            var shipCondition = GetShip(ShipType.Enemy);
            if (shipCondition.gameObject.activeInHierarchy)
            {
                _hazardManager.SkipHazard();
                return;
            }
            
            EventManager.TriggerEvent("EnemyShipSound", null);
            shipCondition.gameObject.SetActive(true);
            shipCondition.transform.position = _spawnPos.position;
            shipCondition.GetComponent<MoveShipUp>().Init();
            shipCondition.Init(ShipType.Enemy, this, message.HazardData.Health);
        }

        private void CreateShipObject(GameObject prefab, ShipType shipType, int health)
        {
            var shipCondition = GetShip(shipType);
            if (shipCondition != null)
            {
                return;
            }
            
            var go = GameObject.Instantiate(prefab, _spawnPos.position, Quaternion.identity);
            go.transform.rotation = Quaternion.Euler(0, 180, 0);
            var ship = go.GetComponent<ShipCondition>();
            ships.Add(ship);
            ship.gameObject.SetActive(false);
        }
    }
}
