using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ScallyWags
{
    [System.Serializable]
    public class KrakenManager
    {
        private KrakenSpawn[] _spawnPos;
        [SerializeField] private Kraken[] _krakens = new Kraken[2];

        public void Init()
        {
            _spawnPos = GameObject.FindObjectsOfType<KrakenSpawn>();
            EventManager.StartListening("Kraken", SpawnKraken);
            EventManager.StartListening("KrakenIntro", SpawnIntroKraken);
        }

        ~KrakenManager()
        {
            EventManager.StopListening("Kraken", SpawnKraken);
            EventManager.StopListening("KrakenIntro", SpawnIntroKraken);
        }
        
        public void Tick()
        {
            foreach (var k in _krakens)
            {
                k?.Tick();
            }
        }

        private void AddKraken(Kraken kraken)
        {
            for (int i = 0; i < _krakens.Length; i++)
            {
                if (_krakens[i] == null)
                {
                    _krakens[i] = kraken;
                    break;
                }
            }
        }

        public void RemoveKraken(Kraken kraken)
        {
            for (int i = 0; i < _krakens.Length; i++)
            {
                if (_krakens[i] == kraken)
                {
                    _krakens[i] = null;
                    break;
                }
            }
        }

        private void SpawnKraken(EventManager.EventMessage message)
        {
            if (IsEnemyShipSpawned()) return;

            if (!CanSpawnKraken()) return;

            for(int i = 0; i <= message.HazardData.NumberOfHazards; i++)
            {
                var t = _spawnPos[i].transform;
                if (t == null) break;
                
                t.rotation = Quaternion.Euler(0, t.localRotation.eulerAngles.y, 0);
                var kraken = GameObject.Instantiate(message.HazardData.Prefab, _spawnPos[i].transform.position, t.rotation);
                
                var krakenComponent = kraken.GetComponent<Kraken>();
                AddKraken(krakenComponent);
                krakenComponent.Init(this);
            }
        }

        private bool CanSpawnKraken()
        {
            bool canSpawn = true;
            for (int i = 0; i < _krakens.Length; i++)
            {
                if (_krakens[i] != null)
                {
                    canSpawn = false;
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
                if (ship.ShipType == ShipManager.ShipType.Enemy)
                {
                    return true;
                }
            }
            return false;
        }

        private void SpawnIntroKraken(EventManager.EventMessage message)
        {
            var t = _spawnPos[1].transform;
            t.rotation = Quaternion.Euler(t.rotation.x, 180, t.rotation.z);
            var kraken = GameObject.Instantiate(message.HazardData.Prefab, t.position, t.rotation);
            
            var krakenComponent = kraken.GetComponent<Kraken>();
            AddKraken(krakenComponent);
            krakenComponent.Init(this);
        }
    }
}