using System;
using UnityEngine;

namespace ScallyWags
{
    [System.Serializable]
    public class KrakenManager : MonoBehaviour
    {
        private KrakenSpawn[] _spawnPos;
        [SerializeField] private Kraken[] _krakens = new Kraken[2];
        private HazardManager _hazardManager;

        public void Init(HazardManager hazardManager)
        {
            _hazardManager = hazardManager;
            _spawnPos = GameObject.FindObjectsOfType<KrakenSpawn>();
        }

        private void OnEnable()
        {
            EventManager.StartListening("CreateKrakens", CreateKrakens);
            EventManager.StartListening("Kraken", SpawnKraken);
            EventManager.StartListening("KrakenIntro", SpawnKraken);
            EventManager.StartListening("RoundOver", KillKrakens);
        }

        private void OnDisable()
        {
            EventManager.StopListening("CreateKrakens", CreateKrakens);
            EventManager.StopListening("Kraken", SpawnKraken);
            EventManager.StopListening("KrakenIntro", SpawnKraken);
            EventManager.StopListening("RoundOver", KillKrakens);
        }
        
        public void Tick()
        {
            foreach (var k in _krakens)
            {
                k?.Tick();
            }
        }

        private void CreateKrakens(EventManager.EventMessage message)
        {
            for (var index = 0; index < _spawnPos.Length; index++)
            {
                var spawn = _spawnPos[index];
                var transform = spawn.transform;
                if (transform == null) break;

                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
                var kraken = GameObject.Instantiate(message.HazardData.Prefab, spawn.transform.position,
                    transform.rotation);

                var krakenComponent = kraken.GetComponent<Kraken>();
                _krakens[index] = krakenComponent;
                krakenComponent.Init(this, 1);
                RemoveKraken(krakenComponent);
            }
        }
        
        private void KillKrakens(EventManager.EventMessage message)
        {
            foreach (var kraken in _krakens)
            {
                kraken.Die();
                RemoveKraken(kraken);
            }
        }

        private bool AddKraken(EventManager.EventMessage message)
        {
            foreach (var kraken in _krakens)
            {
                if (!kraken.gameObject.activeInHierarchy)
                {
                    kraken.gameObject.SetActive(true);
                    kraken.Init(this, message.HazardData.Health);
                    return true;
                }
            }
            return false;
        }

        public void RemoveKraken(Kraken kraken)
        {
            kraken.gameObject.SetActive(false);
        }

        private void SpawnKraken(EventManager.EventMessage message)
        {
            if (IsEnemyShipSpawned()) return;

            if (!CanSpawnKraken())
            {
                _hazardManager.SkipHazard();
                return;
            }

            bool spawnedKraken = false;

            for(int i = 0; i < message.HazardData.NumberOfHazards; i++)
            {
                if (AddKraken(message))
                {
                    spawnedKraken = true;
                }
            }

            if (spawnedKraken)
            {
                EventManager.TriggerEvent("KrakenSound", null);
            }
        }

        private bool CanSpawnKraken()
        {
            bool canSpawn = false;
            foreach (var kraken in _krakens)
            {
                if (!kraken.gameObject.activeInHierarchy)
                {
                    canSpawn = true;
                    break;
                }
            }
            return canSpawn;
        }

        private bool IsEnemyShipSpawned()
        {
            var ships = GameObject.FindObjectsOfType<ShipCondition>();

            foreach (var ship in ships)
            {
                if (ship.ShipType == ShipType.Enemy)
                {
                    return true;
                }
            }
            return false;
        }
    }
}