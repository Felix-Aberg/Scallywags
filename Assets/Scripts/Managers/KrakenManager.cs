using Boo.Lang.Environments;
using Unity.CodeEditor;
using UnityEngine;

namespace ScallyWags
{
    public class KrakenManager
    {
        private KrakenSpawn[] _spawnPos;

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

        private void SpawnKraken(EventManager.EventMessage message)
        {
            foreach (var pos in _spawnPos)
            {
                var t = pos.transform;
                t.rotation = Quaternion.Euler(t.rotation.x, 180, t.rotation.z);
                GameObject.Instantiate(message.HazardData.Prefab, pos.transform.position, t.rotation);
            }
        }

        private void SpawnIntroKraken(EventManager.EventMessage message)
        {
            var t = _spawnPos[1].transform;
            t.rotation = Quaternion.Euler(t.rotation.x, 180, t.rotation.z);
            GameObject.Instantiate(message.HazardData.Prefab, t.position, t.rotation);
        }
    }
}