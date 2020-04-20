using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class MortarManager
    {
        private MortarSpawn[] _mortarSpawns;
        private List<MortarSpawn> _spawnList = new List<MortarSpawn>();
        private MortarSpawn _lastSpawn;
        private AudioSourcePoolManager _audioPool;

        public void Init()
        {
            _mortarSpawns = GameObject.FindObjectsOfType<MortarSpawn>();
            EventManager.StartListening("Mortar", BarrageRequest);
            _audioPool = GameObject.FindObjectOfType<AudioSourcePoolManager>();
        }

        ~MortarManager()
        { 
            EventManager.StopListening("Mortar", BarrageRequest);
        }
        
        private void BarrageRequest(EventManager.EventMessage message)
        {
            Barrage(message.HazardData);
        }

        private void Barrage(HazardData data)
        {
            _lastSpawn = SelectPosition();
            GameObject.Instantiate(data.Prefab, _lastSpawn.transform.position, Quaternion.identity);
            _audioPool.PlayAudioEvent(data.Audio);
            _spawnList.Remove(_lastSpawn);

        }

        private MortarSpawn SelectPosition()
        {
            if (_spawnList.Count <= 0)
            {
                AddSpawnsToList();
            }
            var index = Random.Range(0, _spawnList.Count);
            return _spawnList[index];
        }

        private void AddSpawnsToList()
        {
            foreach (var s in _mortarSpawns)
            {
                _spawnList.Add(s);
            }
        }
    }
}