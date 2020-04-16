using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class HazardManager : MonoBehaviour
    {
        [SerializeField] private List<HazardData> _easyHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _mediumHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _hardHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _introduction = new List<HazardData>();
        [SerializeField] private HazardRating _currentHazardRating;

        [SerializeField] private float _hazardInterval = 10f;
        private float _timer = 0;

        private enum HazardRating
        {
            Easy,
            Medium,
            Hard
        }

        public void Init()
        {
            if (_easyHazards.Count == 0 || _mediumHazards.Count == 0 || _hardHazards.Count == 0 ||
                _introduction.Count == 0)
            {
                Debug.LogError("Missing hazards from lists");
            }
        }

        public void Tick()
        {
            if (_easyHazards.Count == 0 && _mediumHazards.Count == 0 && _hardHazards.Count == 0 &&
                _introduction.Count == 0)
            {
                return;
            }

            _timer += Time.deltaTime;
            if (_timer < _hazardInterval) return;
            _timer = 0;

            if (_introduction.Count > 0)
            {
                SpawnIntroHazard(_introduction);
            }
            else
            {
                ChooseRating();
            }
        }

        private void ChooseRating()
        {
            _currentHazardRating = (HazardRating) Random.Range(0, 3);

            switch (_currentHazardRating)
            {
                case HazardRating.Easy:
                    SpawnHazard(_easyHazards);
                    break;
                case HazardRating.Medium:
                    SpawnHazard(_mediumHazards);
                    break;
                case HazardRating.Hard:
                    SpawnHazard(_hardHazards);
                    break;
                default:
                    Debug.LogError("Hazard rating not set");
                    break;
            }
        }

        private void SpawnHazard(List<HazardData> hazards)
        {
            if (hazards.Count <= 0)
            {
                ChooseRating();
                return;
            }

            var index = Random.Range(0, hazards.Count);
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(hazards[index]);
            EventManager.TriggerEvent(hazards[index].EventName, eventMessage);
        }

        // Introduction hazards come one by one. Remove introduction hazards after triggering hazard
        private void SpawnIntroHazard(List<HazardData> introduction)
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(_introduction[0]);
            EventManager.TriggerEvent(_introduction[0].EventName, eventMessage);
            _introduction.RemoveAt(0);
        }
    }
}