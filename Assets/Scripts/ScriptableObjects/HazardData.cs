using UnityEngine;

namespace ScallyWags
{
    [CreateAssetMenu(menuName = "Hazards")]
    public class HazardData : ScriptableObject
    {
        public string EventName;
        public GameObject Prefab;
        public int Health;
        public AudioEvent Audio;
    }
}