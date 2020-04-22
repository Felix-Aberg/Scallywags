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
            _audioPool = GameObject.FindObjectOfType<AudioSourcePoolManager>();
            EventManager.StartListening("Mortar", BarrageRequest);
            EventManager.StartListening("Intro1", IntroBarrageRequest);
            EventManager.StartListening("Intro2", IntroBarrageRequest);
            EventManager.StartListening("Intro3", IntroBarrageRequestOnTreasure);
        }

        ~MortarManager()
        { 
            EventManager.StopListening("Mortar", BarrageRequest);
            EventManager.StopListening("Intro1", IntroBarrageRequest);
            EventManager.StopListening("Intro2", IntroBarrageRequest);
            EventManager.StartListening("Intro3", IntroBarrageRequestOnTreasure);
        }
        
        private void BarrageRequest(EventManager.EventMessage message)
        {
            Barrage(message.HazardData);
        }

        private void IntroBarrageRequest(EventManager.EventMessage message)
        {
            var middleOfShip = Vector3.zero;
            middleOfShip.y = 30;
            GameObject.Instantiate(message.HazardData.Prefab, middleOfShip, Quaternion.identity);
            _audioPool.PlayAudioEvent(message.HazardData.Audio);
        }
        
        private void IntroBarrageRequestOnTreasure(EventManager.EventMessage message)
        {
            var treasureLocation = GameObject.FindObjectOfType<ScoreItem>().transform.position;
            treasureLocation.y = 30;
            GameObject.Instantiate(message.HazardData.Prefab, treasureLocation, Quaternion.identity);
            _audioPool.PlayAudioEvent(message.HazardData.Audio);
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
            if (Physics.Raycast(_spawnList[index].transform.position, Vector3.down, out var hit))
            {
                if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
                {
                    var spawnPos = _spawnList[index];
                    _spawnList.Remove(spawnPos);
                    SelectPosition();
                }
            }
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