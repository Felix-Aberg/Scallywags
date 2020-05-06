using UnityEngine;

namespace ScallyWags
{
    [CreateAssetMenu(menuName = "Hazards")]
    public class HazardData : ScriptableObject
    {
        public string EventName;
        [Header("Model to spawn")]
        public GameObject Prefab;
        [Header("Health for Kraken and Ship")]
        public int Health;
        [Header("Audio played when events triggers")]
        public AudioEvent Audio;
        [Header("How many hazards should be spawned")]
        public int NumberOfHazards = 1;
    }
}