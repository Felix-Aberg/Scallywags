using UnityEngine;

namespace ScallyWags
{
    public class MortarManager
    {
        private MortarSpawn[] _mortarSpawns;
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
            
        }

        private MortarSpawn SelectPosition()
        {
            var index = Random.Range(0, _mortarSpawns.Length);
            return _mortarSpawns[index];
        }
    }
}