using System.Linq;
using Boo.Lang.Environments;
using Unity.CodeEditor;
using UnityEngine;

namespace ScallyWags
{
    public class KrakenManager
    {
        private KrakenSpawn[] _spawnPos;
        private Kraken[] _krakens = new Kraken[2];

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
            for (int i = 0; i < _krakens.Length; i++)
            {
                if (_krakens[i]?.Health <= 0)
                {
                    _krakens[i] = null;
                }
            }
        }

        private void SpawnKraken(EventManager.EventMessage message)
        {
            if (IsEnemyShipSpawned()) return;
            
            for(int i = 0; i <= message.HazardData.NumberOfHazards; i++)
            {
                if (_krakens[i] != null) continue;
                
                var t = _spawnPos[i].transform;
                t.rotation = Quaternion.Euler(0, t.localRotation.eulerAngles.y, 0);
                var kraken = GameObject.Instantiate(message.HazardData.Prefab, _spawnPos[i].transform.position, t.rotation);
                _krakens[i] = kraken.GetComponent<Kraken>();
            }
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
            GameObject.Instantiate(message.HazardData.Prefab, t.position, t.rotation);
        }
    }
}