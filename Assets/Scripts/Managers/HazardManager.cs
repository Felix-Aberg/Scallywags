using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScallyWags
{
    public class HazardManager : MonoBehaviour
    {
        [SerializeField] private List<HazardData> _easyHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _mediumHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _hardHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _introduction = new List<HazardData>();
        [SerializeField] private HazardRating _currentHazardRating;

        private int _hazardsUnlocked = 1;

        [SerializeField] private float _hazardInterval = 10f;
        private float _timer = 0;
        private FloatVariable _roundTime;

        private enum HazardRating
        {
            Easy,
            Medium,
            Hard
        }

        public void Init(FloatVariable roundTime)
        {
            _roundTime = roundTime;
            
            if (_easyHazards.Count == 0 || _mediumHazards.Count == 0 || _hardHazards.Count == 0 ||
                _introduction.Count == 0)
            {
                Debug.LogError("Missing hazards from lists");
            }
        }

        public void Tick()
        {
            UpdateAvailableHazards();
            
            if (_easyHazards.Count == 0 && _mediumHazards.Count == 0 && _hardHazards.Count == 0 &&
                _introduction.Count == 0)
            {
                Debug.LogError("No hazards");
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

        private void UpdateAvailableHazards()
        {
            if (_roundTime.Value < 400 && _roundTime.Value > 200)
            {
                _hazardsUnlocked = 2;
            }

            if (_roundTime.Value < 200)
            {
                _hazardsUnlocked = 3;
            }
        }

        private void ChooseRating()
        {
            _currentHazardRating = (HazardRating) Random.Range(0, _hazardsUnlocked);

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
            // If no hazards in this rating choose new hazard rating
            if (hazards.Count <= 0)
            {
                ChooseRating();
                return;
            }

            var index = Random.Range(0, hazards.Count);
            for(int i = 0; i < hazards[index].NumberOfHazards; i++)
            {
                EventManager.EventMessage eventMessage = new EventManager.EventMessage(hazards[index]);
                EventManager.TriggerEvent(hazards[index].EventName, eventMessage);   
            }
        }

        // Introduction hazards come one by one. Remove introduction hazards after triggering each hazard
        private void SpawnIntroHazard(List<HazardData> introduction)
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(_introduction[0]);
            EventManager.TriggerEvent(_introduction[0].EventName, eventMessage);
            _introduction.RemoveAt(0);
        }
    }
}