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
            EventManager.StartListening("RoundOver", KillKrakens);
        }

        ~KrakenManager()
        {
            EventManager.StopListening("Kraken", SpawnKraken);
            EventManager.StopListening("KrakenIntro", SpawnIntroKraken);
            EventManager.StopListening("RoundOver", KillKrakens);
        }
        
        public void Tick()
        {
            foreach (var k in _krakens)
            {
                k?.Tick();
            }
        }
        
        private void KillKrakens(EventManager.EventMessage message)
        {
            foreach (var k in _krakens)
            {
                if (k == null) continue;
                k.Die();
            }

            _krakens = new Kraken[2];
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
            
            EventManager.TriggerEvent("KrakenSound", null);

            for(int i = 0; i < message.HazardData.NumberOfHazards; i++)
            {
                var t = _spawnPos[i].transform;
                if (t == null) break;
                
                t.rotation = Quaternion.Euler(0, t.localRotation.eulerAngles.y, 0);
                var kraken = GameObject.Instantiate(message.HazardData.Prefab, _spawnPos[i].transform.position, t.rotation);
                
                var krakenComponent = kraken.GetComponent<Kraken>();
                AddKraken(krakenComponent);
                krakenComponent.Init(this, message.HazardData.Health);
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
                if (ship.ShipType == ShipType.Enemy)
                {
                    return true;
                }
            }
            return false;
        }

        private void SpawnIntroKraken(EventManager.EventMessage message)
        {
            EventManager.TriggerEvent("KrakenSound", null);
            var t = _spawnPos[1].transform;
            t.rotation = Quaternion.Euler(t.rotation.x, 180, t.rotation.z);
            var kraken = GameObject.Instantiate(message.HazardData.Prefab, t.position, t.rotation);
            
            var krakenComponent = kraken.GetComponent<Kraken>();
            AddKraken(krakenComponent);
            krakenComponent.Init(this, message.HazardData.Health);
        }
    }
}